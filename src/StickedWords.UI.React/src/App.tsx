import AppRoutes from './AppRoutes';
import { ErrorListProvider } from './components/ErrorList';
import { AuthProvider } from './components/Login/AuthProvider';
import Navigation from './components/Navigation';

import './App.scss';

function App() {

  return (
    <div className="app">
      <ErrorListProvider>
        <AuthProvider>
          <header>
            <div className="app__logo">Sticked Words</div>
            <div className="app__wrapper">
              <Navigation />
            </div>
          </header>
          <AppRoutes />
        </AuthProvider>
      </ErrorListProvider>
    </div>
  );
}

export default App;
