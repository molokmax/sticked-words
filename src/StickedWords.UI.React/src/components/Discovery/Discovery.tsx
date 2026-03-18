import './Discovery.scss'

import FlashCardList from '../FlashCardList';
import FlashCardListItem from '../FlashCardListItem';
import LearningSessionProgress from '../LearningSessionProgress';
import Loading from '../Loading';

import { ExerciseType, LearningSession, LearningSessionState } from '../../models/LearningSession';
import { FlashCardShort } from '../../models/FlashCardShort';

function Discovery() {
    const session: LearningSession = {
        id: 1,
        state: LearningSessionState.Active,
        exerciseType: ExerciseType.TranslateForeignToNative,
        flashCardCount: 10,
        stages: [
            {id: 1, ordNumber: 0, completedFlashCardCount: 3},
            {id: 2, ordNumber: 1, completedFlashCardCount: 0}
        ]
    };

    const flashCard: FlashCardShort = {
        id: 1,
        word: 'Car',
        translation: 'Машина',
        rate: 65,
        repeatAt: new Date()
    };

    return (
        <>
            <h1>Discovery</h1>

            <h2>Flash Card List</h2>
            <div>
                <FlashCardList></FlashCardList>
            </div>
            
            <h2>Flash Card List Item</h2>
            <div>
                <FlashCardListItem flashCard={flashCard}></FlashCardListItem>
            </div>
            
            <h2>Learning Session Progress</h2>
            <div>
                <LearningSessionProgress session={session}></LearningSessionProgress>
            </div>
            
            <h2>Loading</h2>
            <div>
                <Loading></Loading>
            </div>
        </>
    );
}

export default Discovery;
