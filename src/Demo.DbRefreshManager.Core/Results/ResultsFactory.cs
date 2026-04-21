namespace Demo.DbRefreshManager.Core.Results;

/// <summary>
/// Фабричные методы для Result.
/// </summary>
public static class ResultsFactory
{
    extension(Result _)
    {
        public static Result Success() => new();

        public static Result Failure(Error error) => new(error);

        public static Result<TValue> Success<TValue>(TValue value) => new(value, Error.None);

        public static Result<TValue> Failure<TValue>() => new(default!, Error.None);
    }
}