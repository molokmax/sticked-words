export class PageQuery {

  constructor(includeTotal = true, skip = 0, take: number | undefined = 50) {
    this.skip = skip;
    this.take = take;
    this.includeTotal = includeTotal;
  }

  skip: number;
  take?: number;
  includeTotal: boolean;
}
