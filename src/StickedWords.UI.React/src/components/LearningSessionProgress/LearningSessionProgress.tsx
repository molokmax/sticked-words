import { LearningSession } from '../../models/LearningSession';
import { LearningSessionStage } from '../../models/LearningSessionStage';

import './LearningSessionProgress.scss';


interface Props {
    session: LearningSession
}

function LearningSessionProgress({ session }: Props) {
    
    const getStageCompletePercentage = (stage : LearningSessionStage, session: LearningSession) => {
        const percentage = (stage.completedFlashCardCount / session.flashCardCount) * 100;

        return Math.ceil(percentage) + '%';
    }

    return (
        <div className="learning-session-progress">
            {
                session.stages.map(stage => {
                    return (
                        <div key={stage.id} className="learning-session-progress__stage">
                            <div
                                className="learning-session-progress__stage_completed"
                                style={{ width: getStageCompletePercentage(stage, session) }}
                            ></div>
                        </div>
                    );
                })
            }
        </div>
    );
}

export default LearningSessionProgress;
