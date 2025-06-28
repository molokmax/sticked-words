export class LearningSession {
  exerciseType!: ExerciseType;
  flashCardId?: number;
  flashCardCount!: number;

  static fromJson(json: any): LearningSession {
    const result = new LearningSession();

    result.exerciseType = LearningSession.mapExerciseType(json.exerciseType);
    result.flashCardId = json.flashCardId ?? null;
    result.flashCardCount = json.flashCardCount ?? 0;

    return result;
  }

  private static mapExerciseType(src: string): ExerciseType {
    if (!src) {
      return ExerciseType.None;
    }

    return Object.values(ExerciseType).includes(src as ExerciseType)
      ? src as ExerciseType
      : ExerciseType.None;
  }
}

export enum ExerciseType {
  None = "None",
  TranslateForeignToNative = "TranslateForeignToNative",
  TranslateNativeToForeign = "TranslateNativeToForeign"
}
