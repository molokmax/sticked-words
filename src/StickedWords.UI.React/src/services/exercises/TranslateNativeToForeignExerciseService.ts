import { environments } from "../../environments";
import { Guess } from "../../models/exercises/Guess";
import { GuessResult } from "../../models/exercises/GuessResult";
import { TranslateExercise } from "../../models/exercises/TranslateExercise";

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

  async check(request: Guess): Promise<GuessResult> {
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

    return GuessResult.fromJson(json);
  }
}
