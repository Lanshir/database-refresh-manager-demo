import { ChangeEvent, useState } from 'react';

/**
 * Использовать локальный state для строковых Input/TextArea.
 * 
 * @param defaultState Значение state по ум.
 * @param filter Ф-я фильтрации ввода текста при OnChange.
 * @param callback Коллбэк после сета значения.
 */
export function useTextInputState(
    defaultState: string = '',
    filter = (text: string) => text,
    callback = (text: string) => { }
): [typeof state, typeof setState, typeof onChange] {

    const [state, setState] = useState(defaultState);

    const onChange = (event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => {
        const value = filter(event.target.value);

        setState(value);
        callback(value);
    };

    return [state, setState, onChange];
}

export default useTextInputState;