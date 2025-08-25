import { environments } from "@/environments";
import { TranslateGuess } from "@/models/exercises/TranslateGuess";
import { TranslateGuessResult } from "@/models/exercises/TranslateGuessResult";
import { TranslateExercise } from "@/models/TranslateExercise";

export class TranslateForeignToNativeExerciseService {

  private readonly _baseUrl = `${environments.API_BASE_URL}/exercises/translate-to-native`;

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

  async check(request: TranslateGuess): Promise<TranslateGuessResult> {
    const response = await fetch(this._baseUrl, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(request)
    });
    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }

    const json = await response.json();

    return TranslateGuessResult.fromJson(json);
  }
}
