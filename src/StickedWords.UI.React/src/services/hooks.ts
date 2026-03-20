import { useEffect } from 'react';

export const useEnterKey = (callback: () => void) => {
    useEffect(() => {
        function handleKeyDown(ev: KeyboardEvent) {
            if (ev.key === 'Enter') {
                callback();
            }
        };

        window.addEventListener('keydown', handleKeyDown);
        
        return () => window.removeEventListener('keydown', handleKeyDown);
    }, [callback]);
}