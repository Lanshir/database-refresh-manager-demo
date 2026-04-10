import { AxiosClient } from '@apiClients';
import AuthUrls from '@constants/api/authorizationUrls';
import { LoginInput, LoginResult } from '@shared/types/api/authorize';

/**
 * Авторизация.
 * @param input Ввод авторизации.
 * @returns Результат авторизации.
 */
export async function Authorize(input: LoginInput) {
    const response = await AxiosClient
        .post<LoginResult>(AuthUrls.authorize, input);

    return response.data;
}

/**
 * Проверка авторизации.
 * @returns Данные авторизации.
 */
export async function CheckAuth() {
    const response = await AxiosClient
        .get<LoginResult>(AuthUrls.checkAuth, { timeout: 5000 });

    return response.data;
}

/** Деавторизация. */
export async function Deauthorize() {
    const url = AuthUrls.deauthorize;
    const response = await AxiosClient.delete(url);
}
