import { AxiosClient } from '@apiClients';
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

    const response = await AxiosClient
        .post<ApiResponse<LoginResult>>(url, input);

    return response.data;
}

/**
 * Проверка авторизации.
 * @returns Данные авторизации.
 */
export async function CheckAuth() {
    const url = AuthUrls.checkAuth;

    const response = await AxiosClient
        .get<ApiResponse<LoginResult>>(url, { timeout: 5000 });

    return response.data;
}

/** Деавторизация. */
export async function Deauthorize() {
    const url = AuthUrls.deauthorize;
    const response = await AxiosClient.delete<ApiResponse>(url);

    return response.data;
}
