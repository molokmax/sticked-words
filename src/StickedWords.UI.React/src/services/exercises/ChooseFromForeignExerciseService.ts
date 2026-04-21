import { environments } from "../../environments";
import { ChooseExercise } from "../../models/exercises/ChooseExercise";
import { Guess } from "../../models/exercises/Guess";
import { GuessResult } from "../../models/exercises/GuessResult";

export class ChooseFromForeignExerciseService {

  private readonly _baseUrl = `${environments.API_BASE_URL}/exercises/choose-from-foreign`;

  async get(flashCardId: number): Promise<ChooseExercise> {
    const params = new URLSearchParams();
    params.append('flashCardId', String(flashCardId));
    const url = `${this._baseUrl}?${params}`;
    const response = await fetch(url);
    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }

    const json = await response.json();

    return ChooseExercise.fromJson(json);
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
