import { ChangeEvent } from 'react';
import { useAtom, PrimitiveAtom } from 'jotai';

/**
 * Использовать атом Jotai для строковых Input/TextArea.
 * 
 * @param jotaiAtom Атом Jotai для поля ввода.
 * @param filter Ф-я фильтрации ввода текста при OnChange.
 * @param callback Коллбэк после сета значения.
 */
export function useJotaiTextInputState<TState>(
    jotaiAtom: PrimitiveAtom<TState | string>,
    filter = (text: string) => text,
    callback = (text: string) => { }
): [typeof state, typeof setState, typeof onChange] {
    const [state, setState] = useAtom(jotaiAtom);

    if (typeof state !== 'string') {
        throw Error(`Incorrect input state type. Expected string, provided ${typeof state}.`);
    }

    const onChange = (event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => {
        const value = filter(event.target.value);

        setState(value);
        callback(value);
    };

    return [state, setState, onChange];
}

export default useJotaiTextInputState;