import { ApiResponse } from '@shared/types/api/rest';

/**
 * Проверка флага успешного выполнения в ответе, генерация exception если false.
 * @param apiResponse Ответ api сайта.
 */
export default function EnsureApiResponseSuccess<TData>(apiResponse: ApiResponse<TData>) {
    if (apiResponse?.isSuccess) {
        return apiResponse;
    }

    throw Error(apiResponse?.message);
}