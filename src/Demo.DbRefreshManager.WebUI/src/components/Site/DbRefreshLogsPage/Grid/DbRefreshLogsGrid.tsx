import { FC, useCallback } from 'react';
import { useMount } from 'react-use';
import { useAtomValue, useSetAtom } from 'jotai';
import { Box } from '@mui/material';
import { DataGrid, GridColDef, GridRenderCellParams } from '@mui/x-data-grid';
import { logsListState, logsLoadingState } from '@store/dbRefreshLogs/dbRefreshLogsState';
import { loadDbRefreshLogsQuery } from '@store/dbRefreshLogs/dbRefreshLogsActions';
import { DbRefreshLog } from '@shared/types/api/dbRefreshJobs';
import { nameof } from '@helpers';
import { NameWithNumberComparer } from '@infrastructure/muiDataGrid/cellSortComparers';
import dayjs from 'dayjs';

/**
 * Грид с логами перезаливок БД. 
 */
const DbRefreshLogsGrid: FC = () => {
    const loadLogs = useSetAtom(loadDbRefreshLogsQuery);
    const isLoading = useAtomValue(logsLoadingState);
    const logs = useAtomValue(logsListState);

    useMount(loadLogs);

    return (
        <Box className="grid-wrap">
            <DataGrid columns={columns} rows={logs}
                columnHeaderHeight={64}
                getRowId={useCallback(
                    (row: DbRefreshLog) => logs.findIndex(v => v === row) + 1,
                    [logs]
                )}
                getRowHeight={useCallback(() => 'auto', [])}
                getRowClassName={useCallback(() => 'grid-row', [])}
                loading={isLoading}
                disableColumnMenu
                disableRowSelectionOnClick
                disableVirtualization
                hideFooter
            />
        </Box>
    );
};

const columns: GridColDef<DbRefreshLog>[] =
    [
        {
            field: 'legendCell',
            headerClassName: 'legend-header',
            cellClassName: 'legend-cell',
            headerName: '',
            minWidth: 1,
            maxWidth: 1,
            valueGetter: (val, row, col, api) => row.groupCssColor,
            renderCell: ({ value }: GridRenderCellParams<DbRefreshLog, string>) =>
                <Box sx={{
                    backgroundColor: value ?? 'transparent',
                    position: 'absolute',
                    height: '100%',
                    width: '100%',
                    zIndex: '-1'
                }} />
        },
        {
            field: nameof<DbRefreshLog>('dbName'),
            headerName: '№ Базы',
            width: 140,
            sortComparator: NameWithNumberComparer
        },
        {
            field: 'refreshDateCompare',
            headerName: 'Перезаливка',
            headerAlign: 'center',
            cellClassName: 'refresh-dates-cell pre-line-text',
            width: 200,
            sortable: false,
            valueGetter: (val, row, col, api) => {
                const startString = dayjs(row.refreshStartDate).format('dd DD.MM.YY HH:mm:ss');
                const endString = !!row.refreshEndDate
                    ? dayjs(row.refreshEndDate).format('dd DD.MM.YY HH:mm:ss')
                    : '- - - - - - - - - - - - - - - - - -';

                return [
                    `Начало: ${startString}`,
                    `Конец: ${endString}`
                ].join('\n');
            }
        },
        {
            field: 'refreshDuration',
            headerName: 'Длительность',
            headerAlign: 'center',
            align: 'center',
            width: 120,
            valueGetter: (val, row, col, api) => !!row.refreshEndDate
                ? dayjs(row.refreshEndDate).diff(dayjs(row.refreshStartDate)) : 0,
            valueFormatter: (val, row, col, api) => dayjs(val).utc().format('HH:mm:ss')
        },
        {
            field: nameof<DbRefreshLog>('initiator'),
            headerName: 'Инициатор',
            width: 140
        },
        {
            field: nameof<DbRefreshLog>('code'),
            headerName: 'Код',
            align: 'center',
            width: 40
        },
        {
            field: nameof<DbRefreshLog>('result'),
            headerName: 'Результат',
            flex: 1,
            minWidth: 240,
            cellClassName: 'pre-line-text rich-text-cell'
        },
        {
            field: nameof<DbRefreshLog>('error'),
            headerName: 'Ошибка',
            flex: 1,
            minWidth: 240,
            cellClassName: 'pre-line-text rich-text-cell'
        },
        {
            field: nameof<DbRefreshLog>('executedScript'),
            headerName: 'Выполнен скрипт',
            flex: 2,
            minWidth: 240,
            sortable: false,
            cellClassName: 'pre-line-text rich-text-cell'
        }
    ];

export default DbRefreshLogsGrid;