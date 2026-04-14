import { useCallback, useEffect, useMemo, useState } from 'react';
import { ExerciseType, LearningSession as LearningSessionModel, LearningSessionState } from '../../models/LearningSession';
import { ErrorHandler } from '../../services/ErrorHandler';
import { LearningSessionService } from '../../services/LearningSessionService';
import { useErrorListContext } from '../ErrorList';
import TranslateForeignToNativeExercise from '../Exercises/TranslateForeignToNativeExercise';
import TranslateNativeToForeignExercise from '../Exercises/TranslateNativeToForeignExercise';
import LearningSessionProgress from '../LearningSessionProgress';
import LearningSessionResults from '../LearningSessionResults';
import Loading from '../Loading';

import './LearningSession.scss';


function LearningSession() {

    const [loading, setLoading] = useState(false);
    const [isSessionExpired, setIsSessionExpired] = useState(false);
    const [session, setSession] = useState<LearningSessionModel | null>(null);
    const [showIntro, setShowIntro] = useState(false);
    const { addError } = useErrorListContext();
    
    const service = useMemo(() => new LearningSessionService(), []);

    const loadData = useCallback((isSessionExpired = false) => {
        if (isSessionExpired) {
            setIsSessionExpired(isSessionExpired);
            return;
        }

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
    }, [service, addError]);

    const getExerciseElement = (session: LearningSessionModel) => {
        if (session.flashCardId && session.exerciseType === ExerciseType.TranslateForeignToNative) {
            return (
                <TranslateForeignToNativeExercise
                    flashCardId={ session.flashCardId }
                    onNext={ isExpired => loadData(isExpired) }
                ></TranslateForeignToNativeExercise>
            );
        }

        if (session.flashCardId && session.exerciseType === ExerciseType.TranslateNativeToForeign) {
            return (
                <TranslateNativeToForeignExercise
                    flashCardId={ session.flashCardId }
                    onNext={ isExpired => loadData(isExpired) }
                ></TranslateNativeToForeignExercise>
            );
        }
        
        return <div>Unknown Exercise</div>;
    }

    useEffect(() => {
        loadData();
    }, [loadData]);

    if (loading) {
        return <main className="learning-session"><Loading /></main>;
    }

    if (!session) {
        return <main className="learning-session">Unknown Exercise</main>;
    }

    const isSessionCompleted = isSessionExpired
        || session.state === LearningSessionState.Expired
        || session.state === LearningSessionState.Finished;
    if (isSessionCompleted) {
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
