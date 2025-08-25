/**
 *  Стандартынй овтет rest api сайта.
 */
type ApiResponse<TData = unknown> = {
    code: number
    message: string
    isSuccess: boolean
    page?: number
    totalPages?: number
    totalItems?: number
    data?: TData
};

export default ApiResponse;
