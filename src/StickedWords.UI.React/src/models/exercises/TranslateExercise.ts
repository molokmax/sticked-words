export class TranslateExercise {
  word!: string;
  imageId!: number | null;

  static fromJson(json: any): TranslateExercise {
    const result = new TranslateExercise();

    result.word = json.word ?? "";
    result.imageId = json.imageId ?? null;

    return result;
  }
}
