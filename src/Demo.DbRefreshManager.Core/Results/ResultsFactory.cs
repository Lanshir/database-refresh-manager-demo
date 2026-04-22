namespace Demo.DbRefreshManager.Core.Results;

/// <summary>
/// Фабричные методы для Result.
/// </summary>
public static class ResultsFactory
{
    extension(Result _)
    {
        /// <summary>
        /// Создание успешного результата.
        /// </summary>
        public static Result Success() => new();

        /// <summary>
        /// Создание ошибочного результата.
        /// </summary>
        public static Result Failure(Error error) => error;

        /// <summary>
        /// Создание успешного результата со значением.
        /// </summary>
        public static Result<TValue> Success<TValue>(TValue value) => value;

        /// <summary>
        /// Создание ошибочного результата.
        /// </summary>
        public static Result<TValue> Failure<TValue>(Error error) => error;
    }
}