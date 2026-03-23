import { useEffect } from 'react';

import './ErrorList.scss';


export interface Error {
    id: number;
    message: string;
    timestamp: Date;
}

interface ErrorListProps {
    errors: Error[];
    onRemove: (id: number) => void;
    maxVisible?: number;
}

interface ErrorItemProps {
    error: Error;
    onClose: () => void;
}

function ErrorList({ errors, onRemove, maxVisible = 5 }: ErrorListProps) {
    const visibleErrors = errors.slice(0, maxVisible);

    return (
        <div className="error-list">
            {visibleErrors.map(error => (
                <ErrorItem
                    key={error.id}
                    error={error}
                    onClose={() => onRemove(error.id)}
                />
            ))}
        </div>
    );
};

function ErrorItem({ error, onClose }: ErrorItemProps) {
    return (
        <div className="error-item">
            <div className="error-item__content">
                <div className="error-item__message">
                    {error.message}
                </div>
                <button 
                    className="error-item__close"
                    onClick={onClose}
                    aria-label="Закрыть"
                >
                    ✕
                </button>
            </div>
        </div>
    );
};

export default ErrorList;