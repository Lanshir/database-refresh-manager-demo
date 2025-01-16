import { FC, Suspense, lazy } from 'react';
import { Navigate, Routes, Route } from 'react-router-dom';
import SiteRoutes from '@constants/routes';
import Layout from '@components/Site/Layout/Layout';

const NotFound = lazy(() => import('@components/Router/NotFound/NotFound'));
const LoginPage = lazy(() => import('@components/Site/LoginPage/LoginPage'));
const DbRefreshPage = lazy(() => import('@components/Site/DbRefreshPage/DbRefreshPage'));
const DbRefreshLogsPage = lazy(() => import('@components/Site/DbRefreshLogsPage/DbRefreshLogsPage'));

const Router: FC = () =>
    <Routes>
        <Route path={SiteRoutes.home} element={<Layout />}>
            {/* Home */}
            <Route index element={<Suspense><DbRefreshPage /></Suspense>} />
            <Route path={SiteRoutes.homeAlias} element={<Navigate replace to={SiteRoutes.home} />} />

            {/* Db refresh logs page */}
            <Route path={SiteRoutes.dbRefreshLogs} element={<Suspense><DbRefreshLogsPage /></Suspense>} />

            {/* Login page */}
            <Route path={SiteRoutes.login} element={<Suspense><LoginPage /></Suspense>} />

            {/* NotFound page */}
            <Route path="*" element={<Suspense><NotFound /></Suspense>} />
        </Route>
    </Routes>;

export default Router;
