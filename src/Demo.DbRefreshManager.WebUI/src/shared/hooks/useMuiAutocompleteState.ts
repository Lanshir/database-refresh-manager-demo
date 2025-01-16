import { useState } from 'react';

/**
 * Использовать локальный state для MUI autocomplete.
 * @param defaultState Значение state по ум.
 * @param inputFilter Ф-я фильтрации ввода.
 * @param onChangeCallback Callback при вызове onChange.
 * 
 * @returns Состояния, сеттеры значения/текстового ввода, обработчики onChange.
 */
export function useMuiAutocompleteState<TValue>(
    defaultState: TValue | null,
    inputFilter = (text: string) => text,
    onChangeCallback = (value: TValue | null) => { }
):
    [
        typeof state, typeof setState,
        typeof input, typeof setInput,
        typeof onChange, typeof onChangeInput
    ] {
    // Состояние выбранного значения.
    const [state, setState] = useState(defaultState);

    // Состояние текста input.
    const [input, setInput] = useState('');

    // Событие при выборе пункта.
    const onChange = (event: any, option: TValue | null) => {
        setState(option);
        onChangeCallback(option);
    };

    // Событие изменения текста
    const onChangeInput = (event: any, value: string) => {
        setInput(inputFilter(value));
    };

    return [
        state, setState,
        input, setInput,
        onChange, onChangeInput
    ];
}

export default useMuiAutocompleteState;