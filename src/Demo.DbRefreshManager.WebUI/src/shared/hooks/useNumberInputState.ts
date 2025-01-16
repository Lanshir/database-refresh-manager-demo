import { ChangeEvent, useState } from 'react';

/**
 * Использовать локальный state для числовых Input/TextArea.
 * 
 * @param defaultState Значение state по ум.
 * @param filter Ф-я фильтрации ввода.
 * @param callback Коллбэк после сета значения.
 */
export function useNumberInputState(
    defaultState?: number,
    filter = (text: string) => text,
    callback = (val: number) => { }
): [typeof state, typeof setState, typeof onChange] {

    const [state, setState] = useState(defaultState);
    const numberRegexp = /[^0-9]/ig;

    const onChange = (event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => {
        const value = Number(filter(event.target.value?.replaceAll(numberRegexp, '')));

        setState(value);
        callback(value);
    };

    return [state, setState, onChange];
}

export default useNumberInputState;