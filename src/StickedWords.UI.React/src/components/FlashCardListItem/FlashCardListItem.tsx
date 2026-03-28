import { useNavigate } from 'react-router';
import { FlashCardShort } from '../../models/FlashCardShort';

import './FlashCardListItem.scss'


interface Props {
    flashCard: FlashCardShort
}

function FlashCardListItem({ flashCard }: Props) {

    const navigate = useNavigate();
    
    const getRateLevel = (rate: number) => {
        if (rate === 100) {
            return 3;
        }

        if (rate > 50) {
            return 2;
        }

        return 1;
    }

    const getRate = (flashCard: FlashCardShort) => {
        if (flashCard.repeatAt > new Date()) {
            return 100;
        }

        return flashCard.rate;
    }

    const getRateIcon = (rateLevel: number) => {
        if (rateLevel === 1) {
            return { icon: '/low-rate.png', alt: 'low-rate' };
        }
        if (rateLevel === 2) {
            return { icon: '/medium-rate.png', alt: 'medium-rate' };
        }
        if (rateLevel === 3) {
            return { icon: '/high-rate.png', alt: 'high-rate' };
        }
        return { icon: '', alt: '' };
    }

    const onCardClicked = () => {
        navigate(`details/${flashCard.id}`);
    }

    const rate = getRate(flashCard);
    const rateLevel = getRateLevel(rate);
    const rateIcon = getRateIcon(rateLevel);

    return (
        <div
            className="flash-card-list-item"
            onClick={ () => onCardClicked() }
        >
            <div className="flash-card-list-item__title">
                { flashCard.word }
            </div>
            <div className="flash-card-list-item__space"></div>
            <div className="flash-card-list-item__rate">
                <img src={rateIcon.icon} alt={rateIcon.alt} />
            </div>
        </div>
    )
}

export default FlashCardListItem;
