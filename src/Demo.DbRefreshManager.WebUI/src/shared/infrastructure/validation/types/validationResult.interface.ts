/**
 * Интерфейс результата валидации.
 */
export interface IValidationResult<TErrors> {
    /** Есть ошибки. */
    hasErrors: boolean
    /** Объект ошибок. */
    errors: TErrors
}

export default IValidationResult;
