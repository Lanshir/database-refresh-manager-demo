import { ChangeEvent } from 'react';
import { useAtom, PrimitiveAtom } from 'jotai';

/**
 * Использовать атом Jotai для числовых Input.
 * 
 * @param jotaiAtom Атом Jotai для поля ввода.
 * @param filter Ф-я фильтрации ввода.
 * @param callback Коллбэк после сета значения.
 */
export function useJotaiNumberInputState<TState>(
    jotaiAtom: PrimitiveAtom<TState | number>,
    filter = (text: string) => text,
    callback = (val: number) => { }
): [typeof state, typeof setState, typeof onChange] {
    const [state, setState] = useAtom(jotaiAtom);
    const numberRegexp = /[^0-9]/ig;

    if (typeof state !== 'number') {
        throw Error(`Incorrect input state type. Expected number, provided ${typeof state}.`);
    }

    const onChange = (event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => {
        const value = Number(filter(event.target.value?.replaceAll(numberRegexp, '')));

        setState(value);
        callback(value);
    };

    return [state, setState, onChange];
}

export default useJotaiNumberInputState;