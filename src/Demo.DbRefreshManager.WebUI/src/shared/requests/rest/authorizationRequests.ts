import { RequestSender } from '@apiClients';
import AuthUrls from '@constants/api/authorizationUrls';
import { ApiResponse } from '@shared/types/api/rest';
import { LoginInput, LoginResult } from '@shared/types/api/authorize';

/**
 * Авторизация.
 * @param input Ввод авторизации.
 * @returns Результат авторизации.
 */
export async function Authorize(input: LoginInput) {
    const url = AuthUrls.authorize;

    const response = await RequestSender.post(url, input, undefined, undefined, true);
    const result = await response.json();

    return result as ApiResponse<LoginResult>;
}

/**
 * Проверка авторизации.
 * @returns Данные авторизации.
 */
export async function CheckAuth() {
    const url = AuthUrls.checkAuth;

    const response = await RequestSender.get(url, undefined, 5000);
    const result = await response.json();

    return result as ApiResponse<LoginResult>;
}

/** Деавторизация. */
export async function Deauthorize() {
    const url = AuthUrls.deauthorize;

    const response = await RequestSender.delete(url);
    const result = await response.json();

    return result as ApiResponse;
}
