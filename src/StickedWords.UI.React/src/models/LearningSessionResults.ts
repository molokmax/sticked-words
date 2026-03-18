import { LearningSessionState, mapSessionState } from "./LearningSession";

export class LearningSessionResults {
  state!: LearningSessionState;
  flashCardCount!: number;

  static fromJson(json: any): LearningSessionResults {
    const result = new LearningSessionResults();

    result.state = mapSessionState(json.state);
    result.flashCardCount = json.flashCardCount ?? 0;

    return result;
  }
}
