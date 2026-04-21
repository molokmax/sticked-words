export class ChooseExercise {
  word!: string;
  imageId!: number | null;
  options!: string[];

  static fromJson(json: any): ChooseExercise {
    const result = new ChooseExercise();

    result.word = json.word ?? "";
    result.imageId = json.imageId ?? null;
    result.options = json.options ?? [];

    return result;
  }
}
