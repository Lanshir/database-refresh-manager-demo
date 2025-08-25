import { SyntheticEvent } from 'react';

/**
 * Использовать обработчик OnChange для MUI autocomplete.
 * @param handler Ф-я обработчик значения.
 * @param inputHandler Ф-я обработчик значения текстового ввода.
 * @param inputFilter Ф-я фильтрации ввода.
 */
export function useMuiAutocompleteHandler<TValue>(
    handler: (value: TValue | null) => void,
    inputHandler: (text: string) => void,
    inputFilter = (text: string) => text
): [typeof onChange, typeof onChangeInput] {
    // Событие при выборе пункта.
    const onChange = (event: SyntheticEvent, option: TValue | null) => {
        handler(option);
    };

    // Событие изменения текста
    const onChangeInput = (event: SyntheticEvent, value: string) => {
        inputHandler(inputFilter(value));
    };

    return [onChange, onChangeInput];
}

export default useMuiAutocompleteHandler;
