import { NavLink } from 'react-router';
import { useAuthContext } from '../Login/AuthProvider';

import './Navigation.scss'


function Navigation() {

    const { logout } = useAuthContext();

    return (
        <nav className="navigation">
            <NavLink to="/">All Cards</NavLink>
            <a className="logout-link" onClick={logout}>
                <img src="logout.svg" />
            </a>
        </nav>
    );
}

export default Navigation;
