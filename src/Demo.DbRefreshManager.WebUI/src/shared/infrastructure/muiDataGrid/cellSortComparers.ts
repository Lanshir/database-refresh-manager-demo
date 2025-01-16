import { GridComparatorFn } from '@mui/x-data-grid';

/** Сортировщик названий с номерами (прим. БД-1 БД-2). */
export const NameWithNumberComparer: GridComparatorFn<string> = (s1, s2) => {
    const text1 = s1.replaceAll(/[0-9]/g, '');
    const text2 = s2.replaceAll(/[0-9]/g, '');
    const num1 = Number(s1.replaceAll(/[^0-9]/g, ''));
    const num2 = Number(s2.replaceAll(/[^0-9]/g, ''));

    return text1 === text2
        ? num1 - num2
        : text1 > text2 ? 1 : 0;
};