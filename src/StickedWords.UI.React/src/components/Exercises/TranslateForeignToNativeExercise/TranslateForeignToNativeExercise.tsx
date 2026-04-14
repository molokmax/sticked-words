import { useCallback, useEffect, useMemo, useState } from 'react';
import { TranslateGuess } from '../../../models/exercises/TranslateGuess';
import { GuessResult, TranslateGuessResult } from '../../../models/exercises/TranslateGuessResult';
import { ErrorHandler } from '../../../services/ErrorHandler';
import { TranslateForeignToNativeExerciseService } from '../../../services/exercises/TranslateForeignToNativeExerciseService';
import { useEnterKey, useFocus } from '../../../services/hooks';
import { useErrorListContext } from '../../ErrorList';
import Image from '../../Image';

import './TranslateForeignToNativeExercise.scss';


interface Props {
    flashCardId: number,
    onNext: (isSessionExpired: boolean) => void
};

function TranslateForeignToNativeExercise({ flashCardId, onNext }: Props) {

    const [loading, setLoading] = useState(false);
    const [word, setWord] = useState("");
    const [answer, setAnswer] = useState("");
    const [imageId, setImageId] = useState<number | null>(null);
    const [correctAnswer, setCorrectAnswer] = useState<string | null>(null);
    const [isGuessChecked, setIsGuessChecked] = useState(false);
    const [isGuessCorrect, setIsGuessCorrect] = useState(false);
    const [isSessionExpired, setIsSessionExpired] = useState(false);
    const { addError } = useErrorListContext();

    const service = useMemo(() => new TranslateForeignToNativeExerciseService(), []);

    const setCheckResult = (result: TranslateGuessResult) => {
        setIsGuessChecked(true);
        setIsGuessCorrect(result.result === GuessResult.Correct);
        setCorrectAnswer(result.correctTranslation ?? null);
        setIsSessionExpired(result.isExpired);
    }

    const isFormValid = () => answer.trim().length > 0;

    const loadData = useCallback((flashCardId: number) => {
            
        const resetForm = () => {
            setAnswer("");
            setIsGuessChecked(false);
            setIsGuessCorrect(false);
            setCorrectAnswer(null);
            setIsSessionExpired(false);
        }
        
        setLoading(true);
        resetForm();
        service.get(flashCardId)
            .then(exercise => {
                setWord(exercise.word);
                setImageId(exercise.imageId);
            })
            .catch(err => {
                setWord("");
                setImageId(null);
                addError(ErrorHandler.getMessage(err))
            })
            .finally(() => setLoading(false));
    }, [service, addError]);

    const onCheckClicked = () => {
        if (loading || !isFormValid()) {
            return;
        }

        const request : TranslateGuess = {
            flashCardId: flashCardId,
            answer: answer
        };
        setLoading(true);
        service.check(request)
            .then(response => {
                if (response.result === GuessResult.None) {
                    throw new Error(`Guess result '${response.result}' is not supported`);
                }
                setCheckResult(response);
            })
            .catch(err => {
                addError(ErrorHandler.getMessage(err))
            })
            .finally(() => setLoading(false));
    }

    const autoFocusRef = useFocus();
    useEnterKey(isGuessChecked ? () => onNext(isSessionExpired) : onCheckClicked);

    useEffect(() => loadData(flashCardId), [loadData, flashCardId]);

    let resultsElement = null;
    if (isGuessChecked && isGuessCorrect) {
        resultsElement = (
            <div className="translate-native-to-foreign-exercise__correct-result">
                <div className="translate-native-to-foreign-exercise__result-label">{"\u2713"} Right!</div>
            </div>
        );
    } else if (isGuessChecked && !isGuessCorrect) {
        resultsElement = (
            <div className="translate-native-to-foreign-exercise__wrong-result">
                <div className="translate-native-to-foreign-exercise__result-label">{"\u2717"} Wrong</div>
                <div className="translate-native-to-foreign-exercise__result-label translate-native-to-foreign-exercise__correct-answer">
                    { correctAnswer }
                </div>
            </div>
        );
    }

    const buttonsElement = isGuessChecked
        ? (
            <button
                className="translate-foreign-to-native-exercise__check-button primary"
                disabled={ loading }
                onClick={ e => onNext(isSessionExpired) }
            >
                { loading ? 'Loading...' : 'Next' }
            </button>
        )
        : (
            <button
                className="translate-foreign-to-native-exercise__check-button primary"
                disabled={ loading || !isFormValid() }
                onClick={ onCheckClicked }
            >
                { loading ? 'Loading...' : 'Check' }
            </button>
        );

    return (
        <main className="translate-foreign-to-native-exercise">
            <div className="translate-foreign-to-native-exercise__exercise-description">
                Translate to native:
            </div>
            {
                imageId
                    ? (
                        <div className="translate-foreign-to-native-exercise__image-wrapper">
                            <Image
                                readonly={true}
                                imageId={imageId}
                            ></Image>
                        </div>
                    )
                    : null
            }
            <div className="translate-foreign-to-native-exercise__word">
                { word }
            </div>
            <input
                className="translate-foreign-to-native-exercise__answer"
                ref={ autoFocusRef }
                disabled={ isGuessChecked }
                value={ answer }
                onChange={ ev => setAnswer(ev.target.value) }
            />
            <div className="translate-foreign-to-native-exercise__results">
                { resultsElement }
            </div>
            <div className="translate-foreign-to-native-exercise__buttons">
                { buttonsElement }
            </div>
        </main>
    );
}

export default TranslateForeignToNativeExercise;
