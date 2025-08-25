/**
 * Запись лога перезаливки БД.
 */
export type DbRefreshLog = {
    dbRefreshJobId: number
    dbName: string
    refreshStartDate: string
    refreshEndDate?: string
    code?: number
    result?: string
    error?: string
    initiator?: string
    executedScript?: string
    groupCssColor: string
};

/** Массив ключей типа. */
export const DbRefreshLogKeys: Readonly<Array<keyof DbRefreshLog>> = [
    'dbRefreshJobId',
    'dbName',
    'refreshStartDate',
    'refreshEndDate',
    'code',
    'result',
    'error',
    'initiator',
    'executedScript',
    'groupCssColor'
];

export default DbRefreshLog;
