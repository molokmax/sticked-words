export class FlashCardShort {
  id!: number;
  word!: string;
  translation!: string;
  rate!: number;
  repeatAt!: Date;

  static fromJson(json: any): FlashCardShort {
    const result = new FlashCardShort();

    if (json.id !== undefined) result.id = json.id;
    if (json.word !== undefined) result.word = json.word;
    if (json.translation !== undefined) result.translation = json.translation;
    result.rate = json.rate ?? 0;
    result.repeatAt = json.repeatAt ? new Date(json.repeatAt) : new Date();

    return result;
  }
}
