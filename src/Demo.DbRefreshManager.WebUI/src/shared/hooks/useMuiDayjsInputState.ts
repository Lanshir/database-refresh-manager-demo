import dayjs, { Dayjs } from 'dayjs';
import { useState } from 'react';

/**
 * Использовать локальный state для MUI date-picker.
 *
 * @param defaultState Значение state по ум.
 * @param callback Коллбэк после сета значения.
 */
export function useMuiDayjsInputState(
    defaultState: Date | Dayjs | null = null,
    callback = () => {}
): [typeof date, typeof setDate, typeof onChange] {
    const [date, setDate] = useState<Dayjs | null>(
        !!defaultState ? dayjs(defaultState) : null);

    const onChange = (newDate: Dayjs | null) => {
        setDate(newDate);
        callback();
    };

    return [date, setDate, onChange];
}

export default useMuiDayjsInputState;
