export class TranslateGuessResult {
  result!: GuessResult;
  correctTranslation?: string;

  static fromJson(json: any): TranslateGuessResult {
    const result = new TranslateGuessResult();

    result.result = TranslateGuessResult.mapGuessResult(json.result);
    result.correctTranslation = json.correctTranslation ?? null;

    return result;
  }

  private static mapGuessResult(src: string): GuessResult {
    if (!src) {
      return GuessResult.None;
    }

    return Object.values(GuessResult).includes(src as GuessResult)
      ? src as GuessResult
      : GuessResult.None;
  }
}

export enum GuessResult {
  None = "None",
  Correct = "Correct",
  Wrong = "Wrong"
}
