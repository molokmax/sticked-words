export enum Verdict {
  None = "None",
  Correct = "Correct",
  Wrong = "Wrong"
}

export class GuessResult {
  
  result!: Verdict;
  correctTranslation?: string;
  isExpired!: boolean;

  static fromJson(json: any): GuessResult {
    const result = new GuessResult();

    result.result = GuessResult.mapVerdict(json.result);
    result.correctTranslation = json.correctTranslation ?? null;
    result.isExpired = json.isExpired ?? false;

    return result;
  }

  private static mapVerdict(src: string): Verdict {
    if (!src) {
      return Verdict.None;
    }

    return Object.values(Verdict).includes(src as Verdict)
      ? src as Verdict
      : Verdict.None;
  }
}
