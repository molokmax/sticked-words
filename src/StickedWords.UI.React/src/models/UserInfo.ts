export class UserInfo {
  login!: string;
  surname?: string;
  givenName?: string;
  email?: string;
  externalId!: string;
  authProvider!: string;

  static fromJson(json: any): UserInfo {
    const result = new UserInfo();

    result.login = json.login ?? "";
    result.surname = json.surname ?? null;
    result.givenName = json.givenName ?? null;
    result.email = json.email ?? null;
    result.externalId = json.externalId ?? "";
    result.authProvider = json.authProvider ?? "";

    return result;
  }
}