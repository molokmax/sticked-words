import { useRoutes } from 'react-router';
import Discovery from './components/Discovery';
import FlashCardList from './components/FlashCardList';
import AddFlashCard from './components/AddFlashCard';
import LearningSession from './components/LearningSession';
import FlashCardDetails from './components/FlashCardDetails';

function AppRoutes() {
  const element = useRoutes([
    { path: '/', element: <FlashCardList /> },
    { path: 'discovery', element: <Discovery /> },
    { path: 'add', element: <AddFlashCard /> },
    { path: 'details/:flashCardId', element: <FlashCardDetails /> },
    { path: 'session', element: <LearningSession /> },
    { path: '*', element: <div>404</div> }
  ]);

  return element;
}

export default AppRoutes;
