import { createContext, useState, useCallback, useMemo, ReactNode, useContext } from 'react';
import ErrorList, { type Error } from './ErrorList';

interface ErrorListContextValue {
    errors: Error[];
    addError: (message: string | null, options?: AddErrorOptions) => number;
    removeError: (id: number) => void;
}

interface AddErrorOptions {
    id?: number;
    duration?: number;
}

interface ErrorListProviderProps {
    children: ReactNode;
}

const ErrorListContext = createContext<ErrorListContextValue | null>(null);

export const ErrorListProvider = ({ children }: ErrorListProviderProps) => {
    
    const errorListHook = useErrorList();
    const value = useMemo(() => errorListHook, [errorListHook]);

    return (
        <ErrorListContext.Provider value={value}>
            <ErrorList 
                errors={errorListHook.errors}
                onRemove={errorListHook.removeError}
            />
            {children}
        </ErrorListContext.Provider>
    );
};

export const useErrorList = () => {
    const [errors, setErrors] = useState<Error[]>([]);

    const addError = useCallback((message: string | null, options: AddErrorOptions = {}) => {
        if (!message) {
            return 0;
        }

        const {
            duration = 5000,
            id = Date.now() + Math.floor(Math.random() * 100)
        } = options;

        const newError: Error = {
            id,
            message,
            timestamp: new Date()
        };

        setErrors(prev => [newError, ...prev]);

        setTimeout(() => {
            removeError(id);
        }, duration);

        return id;
    }, []);

    const removeError = useCallback((id: number) => {
        setErrors(prev => prev.filter(e => e.id !== id));
    }, []);

    return {
        errors,
        addError,
        removeError
    };
};

export const useErrorListContext = () => {
    const context = useContext(ErrorListContext);
    if (!context) {
        throw new Error('useErrorListContext must be used within ErrorListProvider');
    }

    return context;
};