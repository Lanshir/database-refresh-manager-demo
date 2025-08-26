import { FC, useEffect, useState } from 'react';
import { useSetAtom, useAtomValue } from 'jotai';
import { Autocomplete, Stack, TextField } from '@mui/material';
import { pageLoadingState, jobsListFilterState } from '@store/dbRefresh/dbRefreshState';
import { dbRefreshJobsSortedItemsState, DbRefreshJobListItem } from '@store/listItems/listItemsState';

/**
 * Фильтры списка БД.
 */
const DbListFiltersChunk: FC = () => {
    const jobListItems = useAtomValue(dbRefreshJobsSortedItemsState);
    const isLoading = useAtomValue(pageLoadingState);
    const setFilters = useSetAtom(jobsListFilterState);

    const [filterJobItem, setFilterJobItem] = useState<DbRefreshJobListItem | null>(null);

    useEffect(() => {
        setFilters(prev => ({ ...prev, jobId: filterJobItem?.id }));
    }, [filterJobItem, setFilters]);

    return (
        <Stack direction="row" marginX={2} spacing={1} alignItems="center">
            <Autocomplete className="filter-input"
                size="small" autoComplete autoHighlight
                loading={isLoading}
                disabled={isLoading}
                options={jobListItems}
                getOptionLabel={j => j.dbName}
                value={filterJobItem}
                onChange={(e, job) => setFilterJobItem(job)}
                renderInput={params => <TextField {...params} label="Поиск БД" />}
            />
        </Stack>
    );
};

export default DbListFiltersChunk;
