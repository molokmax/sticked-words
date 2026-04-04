export enum GuessResult {
  None = "None",
  Correct = "Correct",
  Wrong = "Wrong"
}

export class TranslateGuessResult {
  
  result!: GuessResult;
  correctTranslation?: string;
  isExpired!: boolean;

  static fromJson(json: any): TranslateGuessResult {
    const result = new TranslateGuessResult();

    result.result = TranslateGuessResult.mapGuessResult(json.result);
    result.correctTranslation = json.correctTranslation ?? null;
    result.isExpired = json.isExpired ?? false;

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
