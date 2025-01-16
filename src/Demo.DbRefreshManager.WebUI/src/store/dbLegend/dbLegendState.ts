import { atom } from 'jotai';
import { DbGroup } from '@shared/types/api/dbRefreshJobs';

/** Состояние загрузки легенды. */
export const legendLoadingState = atom(false);

/** Легенда БД. */
export const dbLegendState = atom<DbGroup[]>([]);