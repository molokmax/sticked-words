export class PageResult<T> {
  data!: T[];
  total?: number;

  static fromJson<T>(json: any, itemFactory: (json: any) => T): PageResult<T> {
    const result = new PageResult<T>();

    if (json.total !== undefined) {
      result.total = json.total;
    }
    if (json.data?.length != undefined) {
      result.data = (json.data as any[]).map(itemFactory);
    }

    return result;
  }
}
