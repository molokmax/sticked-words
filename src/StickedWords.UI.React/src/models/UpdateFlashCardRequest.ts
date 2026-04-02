export class UpdateFlashCardRequest {
  id!: number;
  word!: string;
  translation!: string;
  imageId!: number | null;
}
