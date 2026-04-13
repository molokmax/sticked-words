import { createContext, useState, useCallback, useMemo, ReactNode, useContext, useEffect } from 'react';
import { UserInfo } from '../../models/UserInfo';
import { AuthService } from '../../services/AuthService';
import { useErrorListContext } from '../ErrorList';
import Login, { LoginProvider } from './Login';
import { ErrorHandler } from '../../services/ErrorHandler';

interface AuthContextValue {
    user: UserInfo | null;
    loading: boolean;
    login: () => void;
    logout: () => void;
    isAuthenticated: boolean;
}

interface AuthProviderProps {
    children: ReactNode;
}

const AuthContext = createContext<AuthContextValue | null>(null);

export const AuthProvider = ({ children }: AuthProviderProps) => {
    
    const [user, setUser] = useState<UserInfo | null>(null);
    const [loading, setLoading] = useState(true);
    const { addError } = useErrorListContext();

    const service = useMemo(() => new AuthService(), []);

    const logout = useCallback(() => {
        service.logout()
            .then(() => setUser(null));
    }, [service]);

    const login = useCallback((provider: LoginProvider) => {
        if (provider === 'yandex') {
            service.openYandexSignin();
        } else {
            throw new Error(`Login provider [${provider}] is not supported`);
        }
    }, [service]);

    useEffect(() => {
        setLoading(true);
        service.me()
            .then(setUser)
            .catch(err => addError(ErrorHandler.getMessage(err)))
            .finally(() => setLoading(false));
    }, []);

    const value: AuthContextValue = {
        user,
        loading,
        login: () => login('yandex'),
        logout,
        isAuthenticated: !!user
    };

    return (
        <AuthContext.Provider value={value}>
            { user ? children : <Login loading={loading} login={() => login('yandex')} /> }
        </AuthContext.Provider>
    );
};

export const useAuthContext = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuthContext must be used within AuthProvider');
    }

    return context;
};