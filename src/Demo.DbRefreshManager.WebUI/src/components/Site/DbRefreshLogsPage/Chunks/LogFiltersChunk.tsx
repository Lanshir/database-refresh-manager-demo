import { FC, useMemo, useState } from 'react';
import { useMount, useUpdateEffect } from 'react-use';
import { useAtomValue, useSetAtom } from 'jotai';
import { dbRefreshJobsSortedItemsState, DbRefreshJobListItem } from '@store/listItems/listItemsState';
import { dbRefreshJobsListQuery } from '@store/listItems/listItemsActions';
import { loadDbRefreshLogsQuery } from '@store/dbRefreshLogs/dbRefreshLogsActions';
import { useMuiDayjsInputState } from '@hooks';
import { FilterAlt } from '@mui/icons-material';
import { Autocomplete, Button, Stack, StackProps, TextField } from '@mui/material';
import { DatePicker } from '@mui/x-date-pickers';
import { debounce } from 'ts-debounce';

/**
 * Фильтры таблицы логов перезаливки БД.
 */
const LogFiltersChunk: FC<StackProps> = (props) => {
    const dbJobsList = useAtomValue(dbRefreshJobsSortedItemsState);
    const loadDbJobsList = useSetAtom(dbRefreshJobsListQuery);

    const loadLogs = useSetAtom(loadDbRefreshLogsQuery);
    const loadLogsDebounced = useMemo(() => debounce(loadLogs, 500), [loadLogs]);

    const [isLoading, setLoading] = useState(false);
    const [jobFilter, onChangeJobFilter] = useState<DbRefreshJobListItem | null>(null);

    const [dayJsDateFilter, , onChangeDateFilter] = useMuiDayjsInputState();
    const dateFilter = useMemo(
        () => dayJsDateFilter?.isValid() ? dayJsDateFilter.toDate() : null,
        [dayJsDateFilter]);

    const onFilterClick = async () => {
        if (!isLoading) {
            setLoading(true);
            await loadLogs(jobFilter?.id, dateFilter);
            setLoading(false);
        }
    };

    useMount(async () => {
        setLoading(true);
        await loadDbJobsList();
        setLoading(false);
    });

    // Авто-перезагрузка списка при обновлении фильтров.
    useUpdateEffect(() => {
        loadLogsDebounced(jobFilter?.id, dateFilter);
    }, [jobFilter, dateFilter]);

    return (
        <Stack {...props} direction="row-reverse" spacing={1}>
            <Button variant="contained" size="small"
                onClick={onFilterClick}
                disabled={isLoading}
            >
                <FilterAlt />
            </Button>
            <DatePicker label="Дата перезаливки"
                className="filter-input"
                value={dayJsDateFilter}
                onChange={onChangeDateFilter}
                disabled={isLoading}
                format="DD.MM.YY"
                slotProps={{
                    textField: { size: 'small' },
                    field: { clearable: true }
                }}
            />
            <Autocomplete className="filter-input"
                size="small" autoComplete autoHighlight
                loading={isLoading}
                disabled={isLoading}
                options={dbJobsList}
                getOptionLabel={j => j.dbName}
                value={jobFilter}
                onChange={(e, job) => onChangeJobFilter(job)}
                renderInput={params => <TextField {...params} label="Название БД" />}
            />
        </Stack>
    );
};

export default LogFiltersChunk;
