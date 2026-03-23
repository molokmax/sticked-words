import { NavLink } from 'react-router';
import AppRoutes from './AppRoutes';
import { ErrorListProvider } from './components/ErrorList';

import './App.scss';

function App() {
  return (
    <div className="app">
      <ErrorListProvider>
        <header>
          <div className="app__logo">Sticked Words</div>
          <div className="app__wrapper">
            <nav>
              <NavLink to="/">All Cards</NavLink>
            </nav>
          </div>
        </header>
        <AppRoutes />
      </ErrorListProvider>
    </div>
  );
}

export default App;
