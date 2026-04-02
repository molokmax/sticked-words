import { useState, MouseEvent } from "react";
import { ImageService } from "../../services/ImageService";
import { CreateImageRequest } from "../../models/CreateImageRequest";
import { ErrorHandler } from "../../services/ErrorHandler";
import { useErrorListContext } from "../ErrorList";

import './Image.scss';


interface Props {
    imageId: number | null;
    onChanged?: (imageId: number | null) => void,
    readonly: boolean;
}

const Image = ({ imageId, onChanged, readonly = false }: Props) => {

    const [loading, setLoading] = useState(false);
    const { addError } = useErrorListContext();
    
    const service = new ImageService();

    const imageUrl = imageId ? service.getUrl(imageId) : "/no-image.jpg";

    const onUploadClicked = () => {
        const filePicker = service.createUploadPicker(
            ['.jpg', '.png'],
            (file: File, base64: string) => {
                const request: CreateImageRequest = {
                    base64Data: base64
                };
                setLoading(true);
                service.upload(request)
                    .then(id => onChanged?.(id))
                    .catch(err => addError(ErrorHandler.getMessage(err)))
                    .finally(() => setLoading(false))
            }
        );
        filePicker.click();
    }

    const onDeleteClicked = (e: MouseEvent) => {
        e.stopPropagation();
        if (imageId) {
            setLoading(true);
        }
    }
    
    return (
        <div
            className={`image ${readonly || loading ? 'disabled' : ''}`}
            onClick={ onUploadClicked }
        >
            {
                imageId
                    ? (
                        <div
                            className="image__delete-button"
                            onClick={ e => onDeleteClicked(e) }
                        >
                            {"\u2715"}
                        </div>
                    )
                    : null
            }
            <div
                className="image__placeholder"
                style={{ backgroundImage: `url('${imageUrl}')` }}
            ></div>
        </div>
    );
}

export default Image;