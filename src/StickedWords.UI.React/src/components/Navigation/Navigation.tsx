import { NavLink } from 'react-router';
import { useAuthContext } from '../Login/AuthProvider';

import './Navigation.scss';


function Navigation() {

    const { logout } = useAuthContext();

    return (
        <nav className="navigation">
            <NavLink to="/">All Cards</NavLink>
            <button className="navigation__logout-link" onClick={logout}>
                <img src="logout.svg" alt="exit"/>
            </button>
        </nav>
    );
}

export default Navigation;
