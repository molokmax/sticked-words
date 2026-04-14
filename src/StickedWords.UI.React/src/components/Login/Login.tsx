import { useEnterKey } from '../../services/hooks';
import Loading from '../Loading';

import './Login.scss';


export type LoginProvider = 'yandex';

interface Props {
    loading: boolean;
    login: (provider: LoginProvider) => void;
}

function Login({ loading, login }: Props) {

    const onYandexClicked = () => login('yandex');

    useEnterKey(onYandexClicked);

    return (
        <main className="login">
            <div className="login__title">
                Welcome to Sticked Words app!
            </div>
            {
                loading
                    ? <Loading />
                    : (
                        <>
                            <div className="login__sub-title">
                                Select sign-in method
                            </div>
                            <div className="login__buttons">
                                <button
                                    className="login__yandex-button"
                                    onClick={ onYandexClicked }
                                >
                                    <img src="ya.svg" alt=""/>
                                    Yandex ID
                                </button>
                            </div>
                        </>
                    )
            }
        </main>
    )
}

export default Login;
