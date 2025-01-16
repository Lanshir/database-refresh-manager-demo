import { atomWithReset } from 'jotai/utils';
import { DbRefreshLog } from '@shared/types/api/dbRefreshJobs';

/** Состояние загрузки логов. */
export const logsLoadingState = atomWithReset(false);

/** Состояние списка логов. */
export const logsListState = atomWithReset<DbRefreshLog[]>([]);