import { useCallback, useEffect, useMemo, useState } from 'react';
import { Guess } from '../../../models/exercises/Guess';
import { GuessResult, Verdict } from '../../../models/exercises/GuessResult';
import { ErrorHandler } from '../../../services/ErrorHandler';
import { ChooseFromNativeExerciseService } from '../../../services/exercises/ChooseFromNativeExerciseService';
import { useEnterKey } from '../../../services/hooks';
import { useErrorListContext } from '../../ErrorList';
import Image from '../../Image';

import './ChooseFromNativeExercise.scss';


interface Props {
    flashCardId: number,
    onNext: (isSessionExpired: boolean) => void
};

function ChooseFromNativeExercise({ flashCardId, onNext }: Props) {

    const [loading, setLoading] = useState(false);
    const [word, setWord] = useState("");
    const [options, setOptions] = useState<string[]>([]);
    const [imageId, setImageId] = useState<number | null>(null);
    const [answer, setAnswer] = useState<string | null>(null);
    const [correctAnswer, setCorrectAnswer] = useState<string | null>(null);
    const [isGuessChecked, setIsGuessChecked] = useState(false);
    const [isGuessCorrect, setIsGuessCorrect] = useState(false);
    const [isSessionExpired, setIsSessionExpired] = useState(false);
    const { addError } = useErrorListContext();

    const service = useMemo(() => new ChooseFromNativeExerciseService(), []);

    const setCheckResult = (result: GuessResult) => {
        setIsGuessChecked(true);
        setIsGuessCorrect(result.result === Verdict.Correct);
        setCorrectAnswer(result.correctTranslation ?? null);
        setIsSessionExpired(result.isExpired);
    }

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
                setOptions(exercise.options);
            })
            .catch(err => {
                setWord("");
                setImageId(null);
                setOptions([]);
                addError(ErrorHandler.getMessage(err))
            })
            .finally(() => setLoading(false));
    }, [service, addError]);

    const onCheckClicked = (answer: string) => {
        const request : Guess = {
            flashCardId: flashCardId,
            answer: answer
        };
        setAnswer(answer);
        setLoading(true);
        service.check(request)
            .then(response => {
                if (response.result === Verdict.None) {
                    throw new Error(`Guess result '${response.result}' is not supported`);
                }
                setCheckResult(response);
            })
            .catch(err => {
                addError(ErrorHandler.getMessage(err))
            })
            .finally(() => setLoading(false));
    }

    const getOptionClass = (word: string) => {
        if (!isGuessCorrect && word === correctAnswer) {
            return 'right';
        }

        if (!isGuessChecked || word !== answer) {
            return '';
        }

        return isGuessCorrect ? 'right' : 'wrong';
    }

    useEnterKey(() => { if (isGuessChecked) onNext(isSessionExpired) });

    useEffect(() => loadData(flashCardId), [loadData, flashCardId]);

    let resultsElement = null;
    if (isGuessChecked && isGuessCorrect) {
        resultsElement = (
            <div className="choose-from-native-exercise__correct-result">
                <div className="choose-from-native-exercise__result-label">{"\u2713"} Right!</div>
            </div>
        );
    } else if (isGuessChecked && !isGuessCorrect) {
        resultsElement = (
            <div className="choose-from-native-exercise__wrong-result">
                <div className="choose-from-native-exercise__result-label">{"\u2717"} Wrong</div>
            </div>
        );
    }

    const buttonsElement = loading || isGuessChecked
        ? (
            <button
                className="choose-from-native-exercise__check-button primary"
                disabled={ loading }
                onClick={ () => onNext(isSessionExpired) }
            >
                { loading ? 'Loading...' : 'Next' }
            </button>
        )
        : null;

    return (
        <main className="choose-from-native-exercise">
            <div className="choose-from-native-exercise__exercise-description">
                Choose translation:
            </div>
            {
                imageId
                    ? (
                        <div className="choose-from-native-exercise__image-wrapper">
                            <Image
                                readonly={true}
                                imageId={imageId}
                            ></Image>
                        </div>
                    )
                    : null
            }
            <div className="choose-from-native-exercise__word">
                { word }
            </div>
            <div className="choose-from-native-exercise__options">
                {
                    options.map(x => (
                        <div
                            className={`choose-from-native-exercise__option ${getOptionClass(x)} ${isGuessChecked ? 'disabled'  : ''}`}
                            onClick={ () => onCheckClicked(x) }
                        >{x}</div>
                    ))
                }
            </div>
            <div className="choose-from-native-exercise__results">
                { resultsElement }
            </div>
            <div className="choose-from-native-exercise__buttons">
                { buttonsElement }
            </div>
        </main>
    );
}

export default ChooseFromNativeExercise;
