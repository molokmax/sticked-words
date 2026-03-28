import { environments } from "../environments";
import type { PageQuery } from "../models/PageQuery";
import { FlashCardShort } from "../models/FlashCardShort";
import { PageResult } from "../models/PageResult";
import type { CreateFlashCardRequest } from "../models/CreateFlashCardRequest";
import { FlashCardDetails } from "../models/FlashCardDetails";

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

  async getById(flashCardId: number): Promise<FlashCardDetails> {
    const url = `${this._baseUrl}/${flashCardId}`;
    const response = await fetch(url);
    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }

    const json = await response.json();

    return FlashCardDetails.fromJson(json);
  }

  async add(request: CreateFlashCardRequest): Promise<FlashCardShort> {
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

    return FlashCardShort.fromJson(json);
  }

  async delete(flashCardId: number): Promise<FlashCardShort> {
    const url = `${this._baseUrl}/${flashCardId}`;
    const response = await fetch(url, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json"
      }
    });
    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }

    const json = await response.json();

    return FlashCardShort.fromJson(json);
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
