export class FlashCardDetails {
  id!: number;
  word!: string;
  translation!: string;
  imageId!: number | null;
  rate!: number;
  repeatAt!: Date;

  static fromJson(json: any): FlashCardDetails {
    const result = new FlashCardDetails();

    if (json.id !== undefined) result.id = json.id;
    if (json.word !== undefined) result.word = json.word;
    if (json.translation !== undefined) result.translation = json.translation;
    if (json.imageId !== undefined) result.imageId = json.imageId;
    result.rate = json.rate ?? 0;
    result.repeatAt = json.repeatAt ? new Date(json.repeatAt) : new Date();

    return result;
  }
}
