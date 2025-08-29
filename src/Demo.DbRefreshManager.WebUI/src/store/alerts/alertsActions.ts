import { atom } from 'jotai';
import { alertsState, INotistackAlert } from './alertsState';
import { IRequestError } from '@shared/types/errors';
import GqlErrors from '@constants/gqlErrorCodes';
import { FormatRequestErrorMessage, GetGqlErrorsFromNetworkError } from '@helpers';

/** Добавить alert в список. */
export const pushAlertAction = atom(null,
    (get, set, alert: INotistackAlert) => {
        set(alertsState, prev => [...prev, alert]);
    });

/** Добавить ошибку в список. */
export const pushErrorAction = atom(null,
    (get, set, error: Error | IRequestError) => {
        const reqError = error as IRequestError;
        const networkGqlErrors = GetGqlErrorsFromNetworkError(reqError.networkError);

        // Не отображать ошибки авторизации GraphQL.
        if (reqError.graphQLErrors?.some(e => e.extensions!['code'] === GqlErrors.Unauthenticated)
            || networkGqlErrors?.some(e => e.extensions['code'] === GqlErrors.Unauthenticated)
        ) {
            return;
        }

        const message = FormatRequestErrorMessage(reqError);

        console.error(message);
        set(alertsState, prev => [...prev, { variant: 'error', message }]);
    });

/** Достать alert из списка. */
export const popAlertAction = atom(
    get => get(alertsState),
    (get, set) => {
        const alerts = get(alertsState);
        const last = alerts.length > 0
            ? alerts[alerts.length - 1]
            : null;

        if (alerts.length > 0) {
            set(alertsState, prev => prev.slice(0, -1));
        }

        return last;
    });
