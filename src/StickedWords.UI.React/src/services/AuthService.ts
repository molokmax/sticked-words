import { environments } from "../environments";
import { UserInfo } from "../models/UserInfo";

export class AuthService {

  private readonly _baseUrl = `${environments.API_BASE_URL}/auth`;

  openYandexSignin() {
    window.location.href = `${this._baseUrl}/login-yandex`;
  }

  async logout() {
    const url = `${this._baseUrl}/logout`;
    const response = await fetch(url,  {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json"
      }
    });
    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }
  }

  async me(): Promise<UserInfo | null> {
    const url = `${this._baseUrl}/me`;
    const response = await fetch(url);
    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }

    const json = await response.json();

    return json ? UserInfo.fromJson(json) : null;
  }
}
