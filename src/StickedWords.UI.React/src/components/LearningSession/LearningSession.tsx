import { useEffect, useState } from 'react';
import { ErrorHandler } from '../../services/ErrorHandler';
import { ExerciseType, LearningSession as LearningSessionModel, LearningSessionState } from '../../models/LearningSession';
import LearningSessionResults from '../LearningSessionResults';
import LearningSessionProgress from '../LearningSessionProgress';
import { LearningSessionService } from '../../services/LearningSessionService';
import TranslateForeignToNativeExercise from '../Exercises/TranslateForeignToNativeExercise';
import TranslateNativeToForeignExercise from '../Exercises/TranslateNativeToForeignExercise';

import './LearningSession.scss'
import Loading from '../Loading';


function LearningSession() {

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [session, setSession] = useState<LearningSessionModel | null>(null);
    
    const service = new LearningSessionService();
    
        
    const loadLearningSession = async () => {
        const sessionId = service.getCurrentSessionId();
        if (sessionId) {
            const session = await service.getById(sessionId);
            if (session) {
                return session;
            }
        }

        const session = await service.getActive() ?? await service.start();
        service.saveCurrentSessionId(session.id);

        return session;
    }

    const loadData = () => {
        setLoading(true);
        loadLearningSession()
            .then(res => {
                setSession(res);
            })
            .catch(err => {
                setSession(null);
                setError(ErrorHandler.getMessage(err));
            })
            .finally(() => setLoading(false));
    }

    useEffect(() => loadData(), []);

    if (loading) {
        return <main className="learning-session"><Loading /></main>;
    }

    if (!session) {
        return <main className="learning-session">Unknown Exercise</main>;
    }

    if (session.state === LearningSessionState.Finished || session.state === LearningSessionState.Expired) {
        return (
            <main className="learning-session">
                <LearningSessionResults sessionId={ session.id }></LearningSessionResults>
            </main>
        );
    }

    let exerciseElement = null;
    if (session.flashCardId && session.exerciseType === ExerciseType.TranslateForeignToNative) {
        exerciseElement = (
            <TranslateForeignToNativeExercise
                flashCardId={ session.flashCardId }
                onNext={ loadData }
            ></TranslateForeignToNativeExercise>
        );
    } else if (session.flashCardId && session.exerciseType === ExerciseType.TranslateNativeToForeign) {
        exerciseElement = (
            <TranslateNativeToForeignExercise
                flashCardId={ session.flashCardId }
                onNext={ loadData }
            ></TranslateNativeToForeignExercise>
        );
    } else {
        exerciseElement = <main className="learning-session">Unknown Exercise</main>;
    }

    return (
        <main className="learning-session">
            <LearningSessionProgress session={ session }></LearningSessionProgress>
            { exerciseElement }
        </main>
    );
}

export default LearningSession;
