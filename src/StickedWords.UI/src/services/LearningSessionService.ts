import { environments } from "@/environments";
import { LearningSession } from "@/models/LearningSession";

export class LearningSessionService {

  private readonly _baseUrl = `${environments.API_BASE_URL}/learning-sessions`;

  async getActive(): Promise<LearningSession | undefined> {
    const response = await fetch(this._baseUrl);
    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }

    const json = await response.json();
    if (!json) {
      return undefined;
    }

    return LearningSession.fromJson(json);
  }

  async start(): Promise<LearningSession> {
    const response = await fetch(this._baseUrl, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      }
    });
    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }

    const json = await response.json();

    return LearningSession.fromJson(json);
  }
}
