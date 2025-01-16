import { atom } from 'jotai';
import { VariantType } from 'notistack';

/** Массив алертов для вывода. */
export const alertsState = atom<INotistackAlert[]>([]);

export interface INotistackAlert {
    variant: VariantType,
    message: string
}