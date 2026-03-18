import { NavLink } from 'react-router';
import AppRoutes from './AppRoutes';

import './App.scss';

function App() {
  return (
    <div className="app">
      <header>
        <div className="app__logo">Sticked Words</div>
        <div className="app__wrapper">
          <nav>
            <NavLink to="/">All Cards</NavLink>
          </nav>
        </div>
      </header>
      <AppRoutes />
    </div>
  );
}

export default App;
