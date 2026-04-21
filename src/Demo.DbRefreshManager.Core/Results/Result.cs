namespace Demo.DbRefreshManager.Core.Results;

/// <summary>
/// Результат выполнения действия в приложении.
/// </summary>
public class Result
{
    /// <summary>
    /// Успешное выполнение операции.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Операция выполнена с ошибкой.
    /// </summary>
    public bool IsFailure { get; }

    /// <summary>
    /// Ошибка выполненения операции.
    /// </summary>
    public Error Error { get; }

    public Result()
    {
        IsSuccess = true;
        IsFailure = false;
        Error = Error.None;
    }

    public Result(Error error)
    {
        IsSuccess = error == Error.None;
        IsFailure = IsSuccess is false;
        Error = error;
    }

    /// <summary>
    /// Неявная конвертация Error в Result.
    /// </summary>
    public static implicit operator Result(Error error) => new(error);
}

/// <inheritdoc cref="Result" />
public sealed class Result<TValue>(TValue value, Error error) : Result(error)
{
    ///// <summary>
    ///// Данные результата выполнения операции.
    ///// </summary>
    public TValue? Value { get; } = value;

    /// <summary>
    /// Неявная конвертация Error в Result.
    /// </summary>
    public static implicit operator Result<TValue>(Error error) => new(default!, error);
}