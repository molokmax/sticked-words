export class FlashCardShort {
  id!: number;
  word!: string;
  translation!: string;

  static fromJson(json: any): FlashCardShort {
    const result = new FlashCardShort();

    if (json.id !== undefined) result.id = json.id;
    if (json.word !== undefined) result.word = json.word;
    if (json.translation !== undefined) result.translation = json.translation;

    return result;
  }
}
