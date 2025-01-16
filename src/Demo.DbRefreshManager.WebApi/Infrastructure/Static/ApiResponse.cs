using Demo.DbRefreshManager.Common.Enums;
using Demo.DbRefreshManager.WebApi.Models.Api;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.Static;

/// <summary>
/// Шорткаты для создания dto ответов апи.
/// </summary>
public class ApiResponse
{
    /// <summary>
    /// Стандартный ответ.
    /// </summary>
    /// <param name="code">Код ответа.</param>
    /// <param name="message">Сообщение.</param>
    public static ApiResponseDto<object> Default(int code, string message = "")
    {
        return new ApiResponseDto<object>
        {
            Code = code,
            Message = message,
            IsSuccess = code == (int)DefaultStatusCodes.Success
        };
    }

    /// <summary>
    /// Стандартный ответ.
    /// </summary>
    /// <typeparam name="TData">Тип вложенных данных.</typeparam>
    /// <param name="code">Код ответа.</param>
    /// <param name="data">Данные.</param>
    /// <param name="message">Сообщение.</param>
    public static ApiResponseDto<TData> Default<TData>
        (int code, TData? data = default, string message = "")
    {
        return new ApiResponseDto<TData>
        {
            Code = code,
            Message = message,
            IsSuccess = code == (int)DefaultStatusCodes.Success,
            Data = data
        };
    }

    /// <summary>
    /// Успех.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public static ApiResponseDto<object> Success(string message = "success")
    {
        return new ApiResponseDto<object>
        {
            Code = (int)DefaultStatusCodes.Success,
            Message = message,
            IsSuccess = true
        };
    }

    /// <summary>
    /// Успех.
    /// </summary>
    /// <typeparam name="TData">Тип вложенных данных.</typeparam>
    /// <param name="data">Данные.</param>
    /// <param name="message">Сообщение.</param>
    public static ApiResponseDto<TData> Success<TData>
        (TData? data = default, string message = "success")
    {
        return new ApiResponseDto<TData>
        {
            Code = (int)DefaultStatusCodes.Success,
            Message = message,
            IsSuccess = true,
            Data = data
        };
    }

    /// <summary>
    /// Ошибка.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public static ApiResponseDto<object> Error(string message = "error")
    {
        return new ApiResponseDto<object>
        {
            Code = (int)DefaultStatusCodes.Error,
            Message = message,
            IsSuccess = false
        };
    }

    /// <summary>
    /// Ошибка.
    /// </summary>
    /// <typeparam name="TData">Тип вложенных данных.</typeparam>
    /// <param name="data">Данные.</param>
    /// <param name="message">Сообщение.</param>
    public static ApiResponseDto<TData> Error<TData>
        (TData? data = default, string message = "error")
    {
        return new ApiResponseDto<TData>
        {
            Code = (int)DefaultStatusCodes.Error,
            Message = message,
            IsSuccess = false,
            Data = data
        };
    }

    /// <summary>
    /// Постраничные данные.
    /// </summary>
    /// <typeparam name="TData">Тип вложенных данных.</typeparam>
    /// <param name="data">Данные.</param>
    /// <param name="page">Номер страницы.</param>
    /// <param name="pageSize">Размер страницы.</param>
    /// <param name="totalCount">Общее кол-во элементов.</param>
    /// <param name="message">Сообщение.</param>
    public static ApiResponseDto<TData> PageData<TData>
        (TData? data = default, int page = 1, int pageSize = 10, int totalCount = 0, string message = "success")
    {
        if (pageSize <= 0)
        {
            throw new ArgumentException("Response page size must be more then 0");
        }

        var pagesCount = (totalCount + pageSize - 1) / pageSize;

        return new ApiResponseDto<TData>
        {
            Code = (int)DefaultStatusCodes.Success,
            Message = message,
            IsSuccess = true,
            Data = data,
            Page = page,
            TotalItems = totalCount,
            TotalPages = pagesCount
        };
    }
}
