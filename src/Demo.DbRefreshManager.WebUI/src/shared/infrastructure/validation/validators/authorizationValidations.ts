import { IValidationResult, TextRequired } from '@validation';
import { ILoginErrors } from '@store/authorization/authorizationState';

/**
 * Валидация ввода для авторизации.
 */
export function ValidateLoginInput(
    login?: string,
    password?: string
): IValidationResult<ILoginErrors> {

    const errors: ILoginErrors = {
        login: TextRequired(login, 'Введите логин'),
        password: TextRequired(password, 'Введите пароль')
    };

    const hasErrors = Object.values(errors).some(v => !!v);

    return { hasErrors, errors };
}