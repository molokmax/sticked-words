export class LearningSessionStage {
  id!: number;
  ordNumber!: number;
  completedFlashCardCount!: number;

  static fromJson(json: any): LearningSessionStage {
    const result = new LearningSessionStage();

    result.id = json.id;
    result.ordNumber = json.ordNumber ?? 0;
    result.completedFlashCardCount = json.completedFlashCardCount ?? 0;

    return result;
  }
}
