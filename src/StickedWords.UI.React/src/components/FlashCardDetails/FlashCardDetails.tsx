import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router';
import { useErrorListContext } from '../ErrorList';
import { ErrorHandler } from '../../services/ErrorHandler';
import { FlashCardDetails as FlashCardDetailsModel } from '../../models/FlashCardDetails';
import { FlashCardService } from '../../services/FlashCardService';
import Loading from '../Loading';

import './FlashCardDetails.scss';


function FlashCardDetails() {
    
    const params = useParams();
    const navigate = useNavigate();

    const [loading, setLoading] = useState(false);
    const [flashCard, setFlashCard] = useState<FlashCardDetailsModel | null>(null);
    const { addError } = useErrorListContext();

    const service = new FlashCardService();

    const loadData = () => {
        const flashCardIdRaw = params.flashCardId;
        if (!flashCardIdRaw) {
            setFlashCard(null);
            addError('Flash Card identifier is not specified');
            return;
        }
        const flashCardId = Number.parseInt(flashCardIdRaw, 10);
        setLoading(true);
        service.getById(flashCardId)
            .then(res => {
                setFlashCard(res);
            })
            .catch(err => {
                setFlashCard(null);
                addError(ErrorHandler.getMessage(err))
            })
            .finally(() => setLoading(false));
    }

    const goToCardList = () => {
        navigate("/");
    }

    const onDeleteClicked = () => {
        if (!flashCard?.id) {
            return;
        }

        setLoading(true);
        service.delete(flashCard.id)
            .then(res => {
                navigate("/");
            })
            .catch(err => {
                addError(ErrorHandler.getMessage(err))
            })
            .finally(() => setLoading(false));
    }

    useEffect(() => loadData(), []);

    if (loading) {
        return <main className="flash-card-details"><Loading /></main>;
    }

    return (
        <div className="flash-card-details">
            <div className="flash-card-details__form">
                <div className="flash-card-details__form-fields">
                    <div className="flash-card-details__form-field">
                        <span className="flash-card-details__form-field-label">Word: </span>
                        <div className="flash-card-details__form-field-value">
                            { flashCard?.word }
                        </div>
                    </div>
                    <div className="flash-card-details__form-field">
                        <span className="flash-card-details__form-field-label">Translation: </span>
                        <div className="flash-card-details__form-field-value">
                            { flashCard?.translation }
                        </div>
                    </div>
                </div>
                <div className="flash-card-details__form-buttons">
                    <button className="add-flash-card__back-button secondary"
                        onClick={ goToCardList }
                    >Back</button>
                    <button className="flash-card-details__delete-button attention"
                        disabled={ loading || !flashCard }
                        onClick={ onDeleteClicked }
                    >Delete</button>
                </div>
            </div>
        </div>
    );
}

export default FlashCardDetails;
