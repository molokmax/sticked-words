import { useEffect, useState } from 'react';
import { ErrorHandler } from '../../../services/ErrorHandler';
import { GuessResult, TranslateGuessResult } from '../../../models/exercises/TranslateGuessResult';
import { TranslateNativeToForeignExerciseService } from '../../../services/exercises/TranslateNativeToForeignExerciseService';
import { TranslateGuess } from '../../../models/exercises/TranslateGuess';
import { useEnterKey } from '../../../services/hooks';

import './TranslateNativeToForeignExercise.scss';


interface Props {
    flashCardId: number,
    onNext: () => void
};

function TranslateNativeToForeignExercise({ flashCardId, onNext }: Props) {

    const [loading, setLoading] = useState(false);
    const [word, setWord] = useState("");
    const [answer, setAnswer] = useState("");
    const [correctAnswer, setCorrectAnswer] = useState<string | null>(null);
    const [isGuessChecked, setIsGuessChecked] = useState(false);
    const [isGuessCorrect, setIsGuessCorrect] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const service = new TranslateNativeToForeignExerciseService();

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
                setError(ErrorHandler.getMessage(err))
            })
            .finally(() => setLoading(false));
    }

    const onCheckClicked = () => {
        if (!isFormValid()) {
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
                setError(ErrorHandler.getMessage(err))
            })
            .finally(() => setLoading(false));
    }

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
                className="translate-native-to-foreign-exercise__check-button primary"
                onClick={ onNext }
            >
                Next
            </button>
        )
        : (
            <button
                className="translate-native-to-foreign-exercise__check-button primary"
                disabled={ !isFormValid() }
                onClick={ onCheckClicked }
            >
                Check
            </button>
        );

    return (
        <main className="translate-native-to-foreign-exercise">
            <div className="translate-native-to-foreign-exercise__exercise-description">
                Translate to native:
            </div>
            <div className="translate-native-to-foreign-exercise__word">
                { word }
            </div>
            <input
                className="translate-native-to-foreign-exercise__answer"
                disabled={ isGuessChecked }
                value={ answer }
                onChange={ ev => setAnswer(ev.target.value) }
            />
            <div className="translate-native-to-foreign-exercise__results">
                { resultsElement }
            </div>
            <div className="translate-native-to-foreign-exercise__buttons">
                { buttonsElement }
            </div>
        </main>
    );
}

export default TranslateNativeToForeignExercise;
