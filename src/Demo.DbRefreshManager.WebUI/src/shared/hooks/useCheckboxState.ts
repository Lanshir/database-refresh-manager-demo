import { ChangeEvent, useState } from 'react';

/**
 * Использовать локальный state для checkbox.
 * @param defaultState Значение state по ум.
 */
export function useCheckboxState(defaultState: boolean = false): [
    typeof state, typeof setState, typeof onChange
] {
    const [state, setState] = useState(defaultState);

    const onChange = (event: ChangeEvent<HTMLInputElement>) => {
        setState(event.target.checked);
    };

    return [state, setState, onChange];
}

export default useCheckboxState;
