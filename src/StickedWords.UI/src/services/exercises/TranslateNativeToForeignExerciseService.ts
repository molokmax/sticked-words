import { environments } from "@/environments";
import { TranslateExercise } from "@/models/TranslateExercise";

export class TranslateNativeToForeignExerciseService {

  private readonly _baseUrl = `${environments.API_BASE_URL}/exercises/translate-to-foreign`;

  async get(flashCardId: number): Promise<TranslateExercise> {
    const params = new URLSearchParams();
    params.append('flashCardId', String(flashCardId));
    const url = `${this._baseUrl}?${params}`;
    const response = await fetch(url);
    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }

    const json = await response.json();

    return TranslateExercise.fromJson(json);
  }
}
