import { useEffect, useState } from 'react';
import { ErrorHandler } from '../../services/ErrorHandler';
import { ExerciseType, LearningSession as LearningSessionModel, LearningSessionState } from '../../models/LearningSession';
import LearningSessionResults from '../LearningSessionResults';
import LearningSessionProgress from '../LearningSessionProgress';
import { LearningSessionService } from '../../services/LearningSessionService';
import TranslateForeignToNativeExercise from '../Exercises/TranslateForeignToNativeExercise';
import TranslateNativeToForeignExercise from '../Exercises/TranslateNativeToForeignExercise';
import Loading from '../Loading';
import { useErrorListContext } from '../ErrorList';

import './LearningSession.scss'


function LearningSession() {

    const [loading, setLoading] = useState(false);
    const [session, setSession] = useState<LearningSessionModel | null>(null);
    const [showIntro, setShowIntro] = useState(false);
    const { addError } = useErrorListContext();
    
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
                const isFirstCardInStage = res.stages.find(x => x.isActive)?.completedFlashCardCount === 0;
                setShowIntro(isFirstCardInStage);
                if (isFirstCardInStage) {
                    setTimeout(() => setShowIntro(false), 3000);
                }
            })
            .catch(err => {
                setSession(null);
                addError(ErrorHandler.getMessage(err));
            })
            .finally(() => setLoading(false));
    }

    const getExerciseElement = (session: LearningSessionModel) => {
        if (session.flashCardId && session.exerciseType === ExerciseType.TranslateForeignToNative) {
            return (
                <TranslateForeignToNativeExercise
                    flashCardId={ session.flashCardId }
                    onNext={ loadData }
                ></TranslateForeignToNativeExercise>
            );
        }

        if (session.flashCardId && session.exerciseType === ExerciseType.TranslateNativeToForeign) {
            return (
                <TranslateNativeToForeignExercise
                    flashCardId={ session.flashCardId }
                    onNext={ loadData }
                ></TranslateNativeToForeignExercise>
            );
        }
        
        return <div>Unknown Exercise</div>;
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

    return (
        <main className="learning-session">
            <LearningSessionProgress session={ session }></LearningSessionProgress>
            {
                showIntro
                    ? <div className="learning-session__get-ready">Get Ready!</div>
                    : getExerciseElement(session)
            }
        </main>
    );
}

export default LearningSession;
