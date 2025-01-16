import { SelectChangeEvent } from '@mui/material';
import { useState } from 'react';

/**
 * Использовать локальный state для MUI Select.
 * 
 * @param defaultValue Состояние по ум.
 * @param valueMapping Функция конвертации значения MenuItem в значение State.
 */
export function useMuiSelectState<TState, TItemValue = string>(
    defaultValue: TState | null,
    valueMapping: (val: TItemValue | string) => TState | null
): [typeof state, typeof setState, typeof onChange] {
    const [state, setState] = useState<TState | null>(defaultValue);

    const onChange = (event: SelectChangeEvent<TItemValue | string>) => {
        const val = event.target.value;

        if (val === '') setState(null);

        setState(valueMapping(val));
    };

    return [state, setState, onChange];
}

export default useMuiSelectState;