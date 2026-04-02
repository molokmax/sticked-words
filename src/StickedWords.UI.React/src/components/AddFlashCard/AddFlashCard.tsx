import { useState } from 'react';
import { useNavigate } from 'react-router';
import Image from '../Image';
import { ErrorHandler } from '../../services/ErrorHandler';
import { FlashCardService } from '../../services/FlashCardService';
import { ImageService } from '../../services/ImageService';
import { CreateFlashCardRequest } from '../../models/CreateFlashCardRequest';
import { useEnterKey, useFocus } from '../../services/hooks';
import { useErrorListContext } from '../ErrorList';

import './AddFlashCard.scss'


function AddFlashCard() {

    const [loading, setLoading] = useState(false);
    const [word, setWord] = useState("");
    const [imageId, setImageId] = useState<number | null>(null);
    const [translation, setTranslation] = useState("");
    const { addError } = useErrorListContext();

    const navigate = useNavigate();
    const service = new FlashCardService();
    const isFormValid = !!word && !!translation;

    function resetForm() {
        setWord("");
        setTranslation("");
        setImageId(null);
        autoFocusRef.current?.focus();
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
            translation: translation,
            imageId: imageId
        };
        setLoading(true);
        service.add(request)
            .then(() => resetForm())
            .catch(err => addError(ErrorHandler.getMessage(err)))
            .finally(() => setLoading(false));
    }

    useEnterKey(onAddClicked);

    const autoFocusRef = useFocus();

    return (
        <main className="add-flash-card">
            <div className="add-flash-card__form">
                <div className="add-flash-card__form-fields">
                    <div className="add-flash-card__image-wrapper">
                        <Image
                            readonly={loading}
                            imageId={imageId}
                            onChanged={id => setImageId(id)}
                        ></Image>
                    </div>
                    <div className="add-flash-card__form-field">
                        <span className="add-flash-card__form-field-label">Word: </span>
                        <input className="add-flash-card__form-field-value"
                            ref={ autoFocusRef }
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
                        onClick={ goToCardList }
                    >Back</button>
                    <button className="add-flash-card__add-button primary"
                        disabled={ loading || !isFormValid }
                        onClick={ onAddClicked }
                    >Add</button>
                </div>
            </div>
        </main>
    );
}

export default AddFlashCard;
