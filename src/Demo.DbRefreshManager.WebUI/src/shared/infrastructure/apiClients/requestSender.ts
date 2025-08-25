import IRequestError from '@shared/types/errors/requestError.interface';
import Navigation from './utils/navigationProvider';

/**
 *  Обёртка для отправки запросов.
 */
const RequestSender = {
    /**
     * Выполнить GET запрос.
     * @param url Url запроса.
     * @param headers Заголовки запроса
     * @param timeoutMs Таймаут запроса.
     * @param skipErrorsHandler Пропустить обработку ошибок.
     * @returns Ответ сервера.
     */
    async get(
        url: string,
        headers?: object,
        timeoutMs?: number,
        skipErrorsHandler: boolean = false
    ): Promise<Response> {
        const [timeoutController, stopTimeout] = getTimeoutController(timeoutMs, url);

        const options: RequestInit = {
            method: 'GET',
            headers: { ...defaultHeaders, ...headers },
            signal: timeoutController?.signal
        };

        return fetch(url, options)
            .then(res => { stopTimeout(); return res; })
            .then(res => handleServerError(res, skipErrorsHandler));
    },

    /**
     * Выполнить POST запрос.
     * @param url Url запроса.
     * @param body Тело запроса.
     * @param headers Заголовки запроса
     * @param timeoutMs Таймаут запроса.
     * @param skipErrorsHandler Пропустить обработку ошибок.
     * @returns Ответ сервера.
     */
    async post(
        url: string,
        body?: object,
        headers?: object,
        timeoutMs?: number,
        skipErrorsHandler: boolean = false
    ): Promise<Response> {
        const [timeoutController, stopTimeout] = getTimeoutController(timeoutMs, url);

        const options: RequestInit = {
            method: 'POST',
            headers: { ...defaultHeaders, ...headers },
            body: JSON.stringify(body),
            signal: timeoutController?.signal
        };

        return fetch(url, options)
            .then(res => { stopTimeout(); return res; })
            .then(res => handleServerError(res, skipErrorsHandler));
    },

    /**
     * Выполнить PUT запрос.
     * @param url Url запроса.
     * @param body Тело запроса.
     * @param headers Заголовки запроса
     * @param timeoutMs Таймаут запроса.
     * @param skipErrorsHandler Пропустить обработку ошибок.
     * @returns Ответ сервера.
     */
    async put(
        url: string,
        body?: object,
        headers?: object,
        timeoutMs?: number,
        skipErrorsHandler: boolean = false
    ): Promise<Response> {
        const [timeoutController, stopTimeout] = getTimeoutController(timeoutMs, url);

        const options: RequestInit = {
            method: 'PUT',
            headers: { ...defaultHeaders, ...headers },
            body: JSON.stringify(body),
            signal: timeoutController?.signal
        };

        return fetch(url, options)
            .then(res => { stopTimeout(); return res; })
            .then(res => handleServerError(res, skipErrorsHandler));
    },

    /**
     * Выполнить DELETE запрос.
     * @param url Url запроса.
     * @param body Тело запроса.
     * @param headers Заголовки запроса
     * @param timeoutMs Таймаут запроса.
     * @param skipErrorsHandler Пропустить обработку ошибок.
     * @returns Ответ сервера.
     */
    async delete(
        url: string,
        body?: object,
        headers?: object,
        timeoutMs?: number,
        skipErrorsHandler: boolean = false
    ): Promise<Response> {
        const [timeoutController, stopTimeout] = getTimeoutController(timeoutMs, url);

        const options: RequestInit = {
            method: 'DELETE',
            headers: { ...defaultHeaders, ...headers },
            body: JSON.stringify(body),
            signal: timeoutController?.signal
        };

        return fetch(url, options)
            .then(res => { stopTimeout(); return res; })
            .then(res => handleServerError(res, skipErrorsHandler));
    },

    /**
     * Выполнить запрос для FormData.
     * @param url Url запроса.
     * @param method Тип метода.
     * @param body Данные формы.
     * @param headers Заголовки запроса
     * @param timeoutMs Таймаут запроса.
     * @param skipErrorsHandler Пропустить обработку ошибок.
     * @returns Ответ сервера.
     */
    async formData(
        url: string,
        method: Method,
        body: FormData,
        headers?: object,
        timeoutMs?: number,
        skipErrorsHandler: boolean = false
    ): Promise<Response> {
        const [timeoutController, stopTimeout] = getTimeoutController(timeoutMs, url);

        const { 'Content-Type': ct, ...otherHeaders } = defaultHeaders;

        const options: RequestInit = {
            method: method,
            // Do not set content type, .net dont like it.
            headers: { ...otherHeaders, ...headers },
            body: body,
            signal: timeoutController?.signal
        };

        return await fetch(url, options)
            .then(res => { stopTimeout(); return res; })
            .then(res => handleServerError(res, skipErrorsHandler));
    },
};

type Method = 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE';

const defaultHeaders = {
    'Accept': 'application/json',
    'Content-Type': 'application/json'
};

/**
 * Создание AbortController для таймаута fetch.
 * @param timeoutMs Таймаут в МС.
 * @param url Url запроса.
 */
const getTimeoutController = (timeoutMs?: number, url?: string): [AbortController | null, () => void] => {

    if (!timeoutMs) {
        return [null, () => { }];
    }

    const controller = new AbortController();

    const timeoutItem = setTimeout(() => {
        if (url) {
            console.error(`request timeout ${url}`);
        }
        controller.abort();
    }, timeoutMs);

    const stopTimeout = () => clearTimeout(timeoutItem);

    return [controller, stopTimeout];
};

const handleServerError = async (response: Response, skipErrorsHandler: boolean) => {
    if (!skipErrorsHandler && !response.ok && !response.redirected) {
        if (response.status === 401) {
            Navigation.toLoginPage();
        }

        const error = new RequestError(`Ошибка сервера (Код: ${response.status})`);

        // Добавляем данные из тела ошибки к error, если есть.
        const hasJson = !!response.headers.get('Content-Type')?.includes('json');

        if (hasJson) {
            try {
                error.responseData = await response.json();
            } catch {
                console.log('unable to parse request error data');
            }
        }

        throw error;
    }

    return response;
};

class RequestError extends Error implements IRequestError {
    constructor(message?: string) {
        super(message);
        Object.setPrototypeOf(this, RequestError.prototype);
    }

    responseData?: object;
}

export default RequestSender as Readonly<typeof RequestSender>;
