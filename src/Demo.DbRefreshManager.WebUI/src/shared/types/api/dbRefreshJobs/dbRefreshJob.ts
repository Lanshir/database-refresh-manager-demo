/** Dto задачи на перезаливку БД. */
export type DbRefreshJob = {
    id: number
    dbName: string
    inProgress: boolean
    scheduleIsActive: boolean
    manualRefreshDate?: string | null
    scheduleRefreshDate: string
    lastRefreshDate: string
    scheduleChangeUser?: string
    scheduleChangeDate?: string
    releaseComment?: string
    userComment?: string
    groupSortOrder: number
    groupCssColor: string
    groupAccessRoles: string[]
};

/** Массив ключей типа. */
export const DbRefreshJobKeys: Readonly<Array<keyof DbRefreshJob>> = [
    'id',
    'dbName',
    'inProgress',
    'scheduleIsActive',
    'manualRefreshDate',
    'scheduleRefreshDate',
    'lastRefreshDate',
    'scheduleChangeUser',
    'scheduleChangeDate',
    'releaseComment',
    'userComment',
    'groupSortOrder',
    'groupCssColor',
    'groupAccessRoles'
];

export default DbRefreshJob;