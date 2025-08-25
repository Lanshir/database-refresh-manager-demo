import { FC, useMemo, useState } from 'react';
import { useMount, useUpdateEffect } from 'react-use';
import { useAtomValue, useSetAtom } from 'jotai';
import { dbRefreshJobsSortedItemsState, DbRefreshJobListItem } from '@store/listItems/listItemsState';
import { dbRefreshJobsListQuery } from '@store/listItems/listItemsActions';
import { loadDbRefreshLogsQuery } from '@store/dbRefreshLogs/dbRefreshLogsActions';
import { useMuiAutocompleteState, useMuiDayjsInputState } from '@hooks';
import { FilterAlt } from '@mui/icons-material';
import { Autocomplete, Button, Stack, StackProps, TextField } from '@mui/material';
import { DatePicker } from '@mui/x-date-pickers';
import { debounce } from 'ts-debounce';

/**
 * Фильтры таблицы логов перезаливки БД.
 */
const LogFiltersChunk: FC<StackProps> = (props) => {
    const [isLoading, setLoading] = useState(false);
    const dbJobsList = useAtomValue(dbRefreshJobsSortedItemsState);
    const loadDbJobsList = useSetAtom(dbRefreshJobsListQuery);
    const loadLogs = useSetAtom(loadDbRefreshLogsQuery);

    const [dayJsDateFilter, , onChangeDateFilter] = useMuiDayjsInputState();

    const dateFilter = useMemo(
        () => dayJsDateFilter?.isValid() ? dayJsDateFilter.toDate() : null,
        [dayJsDateFilter]);

    const [
        job, , dbNameFilter, , onChangeJobId, onChangeDbNameFilter
    ] = useMuiAutocompleteState<DbRefreshJobListItem>(null);

    const onFilterClick = async () => {
        if (!isLoading) {
            setLoading(true);
            await loadLogs(job?.id, dateFilter);
            setLoading(false);
        }
    };

    const loadLogsDebounced = useMemo(() =>
        debounce(loadLogs, 500), [loadLogs]);

    useMount(async () => {
        setLoading(true);
        await loadDbJobsList();
        setLoading(false);
    });

    // Авто-перезагрузка списка при обновлении фильтров.
    useUpdateEffect(() => {
        loadLogsDebounced(job?.id, dateFilter);
    }, [job?.id, dateFilter]);

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
                options={dbJobsList}
                loading={isLoading}
                disabled={isLoading}
                getOptionLabel={j => j.dbName}
                onChange={onChangeJobId}
                value={job}
                onInputChange={onChangeDbNameFilter}
                inputValue={dbNameFilter}
                size="small" autoComplete autoHighlight
                renderInput={params => <TextField {...params} label="Название БД" />}
            />
        </Stack>
    );
};

export default LogFiltersChunk;
