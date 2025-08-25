import { FC, useState } from 'react';
import { useAtom, useAtomValue } from 'jotai';
import { useMuiAutocompleteHandler } from '@hooks';
import { Autocomplete, Stack, TextField } from '@mui/material';
import { pageLoadingState, jobsListFilterState } from '@store/dbRefresh/dbRefreshState';
import { dbRefreshJobsSortedItemsState, DbRefreshJobListItem } from '@store/listItems/listItemsState';

/**
 * Фильтры списка БД.
 */
const DbListFiltersChunk: FC = () => {
    const jobListItems = useAtomValue(dbRefreshJobsSortedItemsState);
    const isLoading = useAtomValue(pageLoadingState);
    const [filters, setFilters] = useAtom(jobsListFilterState);

    const [jobInput, setJobInput] = useState('');
    const [onChangeJob, onChangeJobInput] = useMuiAutocompleteHandler<DbRefreshJobListItem>(
        job => setFilters(prev => ({ ...prev, jobId: job?.id })),
        input => setJobInput(input));

    return (
        <Stack direction="row" marginX={2} spacing={1} alignItems="center">
            <Autocomplete className="filter-input"
                options={jobListItems}
                loading={isLoading}
                disabled={isLoading}
                getOptionLabel={j => j.dbName}
                onChange={onChangeJob}
                value={jobListItems.find(j => j.id === filters.jobId) ?? null}
                onInputChange={onChangeJobInput}
                inputValue={jobInput}
                size="small" autoComplete autoHighlight
                renderInput={params => <TextField {...params} label="Поиск БД" />}
            />
        </Stack>
    );
};

export default DbListFiltersChunk;
