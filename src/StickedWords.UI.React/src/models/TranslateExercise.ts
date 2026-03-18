export class TranslateExercise {
  word!: string;

  static fromJson(json: any): TranslateExercise {
    const result = new TranslateExercise();

    result.word = json.word ?? "";

    return result;
  }
}
