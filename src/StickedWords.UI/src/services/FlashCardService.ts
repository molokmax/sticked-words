import type { PageQuery } from "@/models/PageQuery";
import { FlashCardShort } from "@/models/FlashCardShort";
import { PageResult } from "@/models/PageResult";
import { environments } from "@/environments";

export class FlashCardService {

  private readonly _baseUrl = `${environments.API_BASE_URL}/flash-cards`;

  async getByQuery(query: PageQuery): Promise<PageResult<FlashCardShort>> {
    const url = `${this._baseUrl}?${this.queryToParams(query)}`;
    const response = await fetch(url);
    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }

    const json = await response.json();

    return PageResult.fromJson<FlashCardShort>(json, FlashCardShort.fromJson);
  }

  private queryToParams(query: PageQuery): URLSearchParams {
    const params = new URLSearchParams();
    for (const [key, value] of Object.entries(query)) {
      if (value !== undefined && value !== null) {
        params.append(key, String(value));
      }
    }
    return params;
  }
}
