import { environments } from "@/environments";
import { LearningSession } from "@/models/LearningSession";
import { LocalDataStorage } from "./LocalDataStorage";
import { LearningSessionResults } from "@/models/LearningSessionResults";

export class LearningSessionService {

  private readonly _baseUrl = `${environments.API_BASE_URL}/learning-sessions`;
  private readonly _dataStorage = new LocalDataStorage();
  private readonly _sessionIdStorageKey = 'LEARNING_SESSION_ID';

  async getById(id: number): Promise<LearningSession | undefined> {
    const url = `${this._baseUrl}/${id}`;
    const response = await fetch(url);
    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }

    const json = await response.json();
    if (!json) {
      return undefined;
    }

    return LearningSession.fromJson(json);
  }

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

  async getResults(id: number): Promise<LearningSessionResults> {
    const url = `${this._baseUrl}/${id}/results`;
    const response = await fetch(url);
    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }

    const json = await response.json();

    return LearningSessionResults.fromJson(json);
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

  getCurrentSessionId(): number | undefined {
    return this._dataStorage.get<number>(this._sessionIdStorageKey);
  }

  saveCurrentSessionId(sessionId: number) {
    this._dataStorage.save(this._sessionIdStorageKey, sessionId);
  }

  deleteCurrentSessionId() {
    this._dataStorage.delete(this._sessionIdStorageKey);
  }
}
