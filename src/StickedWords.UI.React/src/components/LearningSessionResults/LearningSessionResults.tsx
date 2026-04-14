import { useCallback, useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router';
import { LearningSessionState } from '../../models/LearningSession';
import { ErrorHandler } from '../../services/ErrorHandler';
import { LearningSessionService } from '../../services/LearningSessionService';
import { useEnterKey } from '../../services/hooks';
import { useErrorListContext } from '../ErrorList';

import './LearningSessionResults.scss';


interface Props {
    sessionId: number
}

function LearningSessionResults({ sessionId }: Props) {

    const [loading, setLoading] = useState(false);
    const [sessionState, setSessionState] = useState(LearningSessionState.None);
    const [cardCount, setCardCount] = useState(0);
    const { addError } = useErrorListContext();

    const navigate = useNavigate();
    const service = useMemo(() => new LearningSessionService(), []);


    const loadData = useCallback((sessionId: number) => {
        setLoading(true);
        service.getResults(sessionId)
            .then(res => {
                setSessionState(res.state);
                setCardCount(res.flashCardCount);
            })
            .catch(err => {
                setSessionState(LearningSessionState.None);
                setCardCount(0);
                addError(ErrorHandler.getMessage(err));
            })
            .finally(() => setLoading(false));
    }, [service, addError])

    const onContinueClicked = () => {
        service.deleteCurrentSessionId();
        navigate("/");
    }

    useEnterKey(onContinueClicked);

    useEffect(() => {
        loadData(sessionId);
    }, [sessionId, loadData]);

    if (sessionState === LearningSessionState.Expired) {
        return (
            <main className="learning-session-results">
                <div className="learning-session-results__title">
                    Session expired
                </div>
                <div className="learning-session-results__buttons">
                    <button
                        className="learning-session-results__ok-button primary"
                        onClick={ onContinueClicked }
                    >
                        OK
                    </button>
                </div>
            </main>
        );
    }

    if (sessionState === LearningSessionState.Finished) {
        return (
            <main className="learning-session-results">
                <div className="learning-session-results__title">
                    Session finished
                </div>
                <div className="learning-session-results__description">
                    Learned words: { cardCount }
                </div>
                <div className="learning-session-results__buttons">
                    <button
                        className={ `learning-session-results__add-button primary ${ loading ? 'disabled' : '' }` }
                        onClick={ onContinueClicked }
                    >
                        Complete
                    </button>
                </div>
            </main>
        );
    }

    return (
        <main className="learning-session-results">
            <div className="learning-session-results__title">
                Unknown session state
            </div>
            <div className="learning-session-results__buttons">
                <button
                    className="learning-session-results__ok-button primary"
                    onClick={ onContinueClicked }
                >
                    OK
                </button>
            </div>
        </main>
    )
}

export default LearningSessionResults;
