export class PageQuery {

  constructor(includeTotal = true, skip = 0, take: number | undefined = 50) {
    this.skip = skip;
    this.take = take;
    this.includeTotal = includeTotal;
  }

  skip: number;
  take?: number;
  includeTotal: boolean;

  // static create(includeTotal = true, skip = 0, take: number | undefined = 50): PageQuery {
  //   let result = new PageQuery();

  //   result.skip = skip;
  //   result.take = take;
  //   result.includeTotal = includeTotal;

  //   return result;
  // }
}
