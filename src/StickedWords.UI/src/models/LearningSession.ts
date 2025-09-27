export class LearningSession {
  id!: number;
  state!: LearningSessionState;
  exerciseType!: ExerciseType;
  flashCardId?: number;
  flashCardCount!: number;

  static fromJson(json: any): LearningSession {
    const result = new LearningSession();

    result.id = json.id;
    result.state = mapSessionState(json.state);
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

export const mapSessionState = (src: string): LearningSessionState => {
  if (!src) {
    return LearningSessionState.None;
  }

  return Object.values(LearningSessionState).includes(src as LearningSessionState)
    ? src as LearningSessionState
    : LearningSessionState.None;
}

export enum ExerciseType {
  None = "None",
  TranslateForeignToNative = "TranslateForeignToNative",
  TranslateNativeToForeign = "TranslateNativeToForeign"
}

export enum LearningSessionState {
  None = "None",
  Active = "Active",
  Finished = "Finished",
  Expired = "Expired"
}
