import { useState } from 'react';
import { useNavigate } from 'react-router';
import { ErrorHandler } from '../../services/ErrorHandler';
import { FlashCardService } from '../../services/FlashCardService';
import { CreateFlashCardRequest } from '../../models/CreateFlashCardRequest';
import { useEnterKey } from '../../services/hooks';

import './AddFlashCard.scss'

function AddFlashCard() {

    const [loading, setLoading] = useState(false);
    const [word, setWord] = useState("");
    const [translation, setTranslation] = useState("");
    const [error, setError] = useState<string | null>(null);

    const navigate = useNavigate();
    const service = new FlashCardService();
    const isFormValid = !!word && !!translation;

        
    function resetForm() {
        setWord("");
        setTranslation("");
    }

    function goToCardList() {
        resetForm();
        navigate("/");
    }
 
    function onAddClicked() {
        if (!isFormValid) {
            return;
        }

        const request : CreateFlashCardRequest = {
            word: word,
            translation: translation
        };
        setLoading(true);
        service.add(request)
            .then(() => resetForm())
            .catch(err => setError(ErrorHandler.getMessage(err)))
            .finally(() => setLoading(false));
    }

    useEnterKey(onAddClicked);

    return (
        <main className="add-flash-card">
            <div className="add-flash-card__form">
                <div className="add-flash-card__form-fields">
                    <div className="add-flash-card__form-field">
                        <span className="add-flash-card__form-field-label">Word: </span>
                        <input className="add-flash-card__form-field-value"
                            value={word}
                            onChange={ ev => setWord(ev.target.value) }
                        />
                    </div>
                    <div className="add-flash-card__form-field">
                        <span className="add-flash-card__form-field-label">Translation: </span>
                        <input className="add-flash-card__form-field-value"
                            value={translation}
                            onChange={ ev => setTranslation(ev.target.value) }
                        />
                    </div>
                </div>
                <div className="add-flash-card__form-buttons">
                    <button className="add-flash-card__back-button secondary"
                        onClick={ ev => goToCardList() }
                    >Back</button>
                    <button className="add-flash-card__add-button primary"
                        disabled={ !isFormValid }
                        onClick={ ev => onAddClicked() }
                    >Add</button>
                </div>
            </div>
        </main>
    );
}

export default AddFlashCard;
