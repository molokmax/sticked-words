export class LocalDataStorage {

  get<TData>(key: string): TData | undefined {
    const json = localStorage.getItem(key);
    if (!json) {
      return undefined;
    }

    return JSON.parse(json) as TData;
  }

  save<TData>(key: string, data: TData) {
    const json = JSON.stringify(data);
    localStorage.setItem(key, json);
  }

  delete(key: string) {
    localStorage.removeItem(key);
  }
}
