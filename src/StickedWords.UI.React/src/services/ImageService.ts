import { environments } from "../environments";
import { CreateImageRequest } from "../models/CreateImageRequest";

export class ImageService {

  private readonly _baseUrl = `${environments.API_BASE_URL}/images`;

  getUrl(imageId: number): string {
    return `${this._baseUrl}/${imageId}`;
  }

  async upload(request: CreateImageRequest): Promise<number> {
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

    const imageId = Number.parseInt(await response.text());

    return imageId;
  }

  async delete(imageId: number): Promise<void> {
    const url = `${this._baseUrl}/${imageId}`;
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

  createUploadPicker(extensions: string[], callback: (file: File, base64: string) => void) {
    const picker = document.createElement("input") as HTMLInputElement;
    picker.type = "file";
    picker.accept = extensions.join(',');
    picker.onchange = (e) => {
      if (e.target && 'files' in e.target) {
        const files = e.target.files as FileList;
        if (!files.length) {
          console.error('File is not selected');
          return;
        }

        const file = files[0];
        this.readBase64(file)
          .then((res: string) => callback(file, res));
      }
    }

    return picker;
  }

  private readBase64(file: File) {
    return new Promise<string>((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result as string);
      reader.onerror = reject;
    });
  }
}
