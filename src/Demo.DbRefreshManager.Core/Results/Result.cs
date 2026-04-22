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

    internal Result()
    {
        IsSuccess = true;
        IsFailure = false;
        Error = Error.None;
    }

    protected Result(Error error)
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
public sealed class Result<TValue> : Result
{
    ///// <summary>
    ///// Данные результата выполнения операции.
    ///// </summary>
    public TValue? Value { get; }

    private Result(TValue value, Error error) : base(error)
    {
        Value = value;
    }

    /// <summary>
    /// Неявная конвертация значения в Result.
    /// </summary>
    public static implicit operator Result<TValue>(TValue value) => new(value, Error.None);

    /// <summary>
    /// Неявная конвертация Error в Result.
    /// </summary>
    public static implicit operator Result<TValue>(Error error) => new(default!, error);
}