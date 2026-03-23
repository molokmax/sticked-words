import { useEffect, useState } from 'react';
import { ErrorHandler } from '../../../services/ErrorHandler';
import { GuessResult, TranslateGuessResult } from '../../../models/exercises/TranslateGuessResult';
import { TranslateForeignToNativeExerciseService } from '../../../services/exercises/TranslateForeignToNativeExerciseService';
import { TranslateGuess } from '../../../models/exercises/TranslateGuess';
import { useEnterKey, useFocus } from '../../../services/hooks';
import { useErrorListContext } from '../../ErrorList';

import './TranslateForeignToNativeExercise.scss';


interface Props {
    flashCardId: number,
    onNext: () => void
};

function TranslateForeignToNativeExercise({ flashCardId, onNext }: Props) {

    const [loading, setLoading] = useState(false);
    const [word, setWord] = useState("");
    const [answer, setAnswer] = useState("");
    const [correctAnswer, setCorrectAnswer] = useState<string | null>(null);
    const [isGuessChecked, setIsGuessChecked] = useState(false);
    const [isGuessCorrect, setIsGuessCorrect] = useState(false);
    const { addError } = useErrorListContext();

    const service = new TranslateForeignToNativeExerciseService();

    const resetForm = () => {
        setAnswer("");
        setIsGuessChecked(false);
        setIsGuessCorrect(false);
        setCorrectAnswer(null);
    }

    const setCheckResult = (result: TranslateGuessResult) => {
        setIsGuessChecked(true);
        setIsGuessCorrect(result.result === GuessResult.Correct);
        setCorrectAnswer(result.correctTranslation ?? null);
    }

    const isFormValid = () => answer.trim().length > 0;

    const loadData = (flashCardId: number) => {
        setLoading(true);
        resetForm();
        service.get(flashCardId)
            .then(exercise => setWord(exercise.word))
            .catch(err => {
                setWord("");
                addError(ErrorHandler.getMessage(err))
            })
            .finally(() => setLoading(false));
    }

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
    useEnterKey(isGuessChecked ? onNext : onCheckClicked);

    useEffect(() => loadData(flashCardId), [flashCardId]);

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
                onClick={ onNext }
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
