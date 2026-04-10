import { atom } from 'jotai';
import { RESET } from 'jotai/utils';
import { Location, NavigateFunction } from 'react-router';
import {
    authorizationState, loginLoadingState, loginErrorsState, alertState
} from './authorizationState';
import { ValidateLoginInput } from '@validation/validators/authorizationValidations';
import { Authorize, CheckAuth, Deauthorize } from '@requests/rest/authorizationRequests';
import { pushErrorAction } from '@store/alerts/alertsActions';
import { IRequestError } from '@shared/types/errors';
import { ProblemDetails } from '@shared/types/api/rest/problemDetails.interface';
import Routes from '@constants/routes';

/** Запрос авторизации. */
export const authorizeQuery = atom(null,
    async (get, set,
        login: string,
        password: string,
        rememberMe: boolean,
        onSuccess: () => void
    ) => {
        try {
            set(alertState, { text: '' });

            const { errors, hasErrors } = ValidateLoginInput(login, password);

            if (hasErrors) {
                set(loginErrorsState, errors);
                return;
            }

            set(loginLoadingState, true);

            const result = await Authorize({ login, password, rememberMe });

            set(authorizationState, {
                isAuthorized: true,
                user: {
                    login: result.login,
                    fullName: result.fullName,
                    roles: result.roles
                }
            });

            onSuccess();
        }
        catch (e) {
            const err = e as IRequestError<ProblemDetails>;
            const message = err.response?.data?.detail ?? err.message;

            set(alertState, { severity: 'error', text: message });
        }
        finally {
            set(loginLoadingState, false);
        }
    });

/** Запрос проверки авторизации. */
export const checkAuthQuery = atom(null,
    async (get, set, navigate: NavigateFunction, location: Location) => {
        try {
            const result = await CheckAuth();

            set(authorizationState, {
                isAuthorized: true,
                user: {
                    login: result.login,
                    fullName: result.fullName,
                    roles: result.roles
                }
            });

            if (location.pathname.match(Routes.login)) {
                navigate({ pathname: Routes.home }, { replace: true });
            }
        }
        catch {
            set(authorizationState, RESET);
            navigate({ pathname: Routes.login }, { replace: true });
        }
    });

/** Запрос деавторизации. */
export const deauthorizeQuery = atom(null,
    async (get, set, onSuccess: () => void) => {
        try {
            await Deauthorize();
            onSuccess();
        }
        catch (e) {
            set(pushErrorAction, e as Error);
        }
        finally {
            set(authorizationState, RESET);
        }
    });
