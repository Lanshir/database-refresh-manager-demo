import { atom } from 'jotai';
import { atomWithReset, RESET } from 'jotai/utils';
import IAlert from '@shared/types/mui/muiAlert.interface';

export const authorizationState = atomWithReset<IAuthState>({
    isAuthorized: false,
    user: {
        login: '',
        fullName: '',
        roles: []
    }
});

/** Состояние загрузки авторизации. */
export const loginLoadingState = atom(false);

/** Состояние ошибок валидации формы авторизации. */
export const loginErrorsState = atomWithReset<ILoginErrors>({});

/** Атом alert'a авторизации. */
const alertAtom = atomWithReset<IAlert>({});

/** Состояние alert авторизации. */
export const alertState = atom(
    get => get(alertAtom),
    (get, set, newAlert: IAlert | typeof RESET) => {
        newAlert === RESET
            ? set(alertAtom, RESET)
            : set(alertAtom, prev => ({ ...prev, ...newAlert }));
    }
);

export interface IAuthState {
    isAuthorized: boolean
    user: {
        login: string
        fullName: string
        roles: Array<string>
    }
}

/** Интерфейс ошибок валидации формы авторизации. */
export interface ILoginErrors {
    login?: string
    password?: string
}
