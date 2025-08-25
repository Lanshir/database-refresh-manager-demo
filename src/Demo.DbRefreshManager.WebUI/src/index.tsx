import '@shared/styles/sass/site.scss';
import 'dayjs/locale/ru';
import { createRoot } from 'react-dom/client';
import { RouterProvider, createBrowserRouter } from 'react-router-dom';
import dayjs from 'dayjs';
import dayjsUtc from 'dayjs/plugin/utc';
import NavigationProvider from '@infrastructure/apiClients/utils/navigationProvider';
import App from './App';
import ConfigLoader from '@components/ConfigLoader/ConfigLoader';
import Router from '@components/Router/Router';
import Routes from '@constants/routes';

// Установка языка day js.
dayjs.locale('ru');
dayjs.extend(dayjsUtc);

const browserRouter = createBrowserRouter([
    { path: '*', Component: Router }
]);

// Конфигурация навигации вне компонентов.
NavigationProvider.toLoginPage = () =>
    browserRouter.navigate({ pathname: Routes.login });

const container = document.getElementById('root');
const root = createRoot(container!);

root.render(
    <App>
        <ConfigLoader />
        <RouterProvider router={browserRouter} />
    </App>
);
