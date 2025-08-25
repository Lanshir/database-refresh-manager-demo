import { GridValueFormatter } from '@mui/x-data-grid';
import { ShortenFullName } from '@helpers';
import dayjs from 'dayjs';

/**
 * Day.js date formatter.
 * @param format DateTime format.
 */
export function DayjsDateFormatter(format: string = 'DD.MM.yyyy') {
    const formatter: GridValueFormatter
        = (value: string) => dayjs(value).format(format);

    return formatter;
}

/** Короткое ФИО (Фамилия И.О). */
export const ShortFullName: GridValueFormatter
    = (value: string) => ShortenFullName(value);
