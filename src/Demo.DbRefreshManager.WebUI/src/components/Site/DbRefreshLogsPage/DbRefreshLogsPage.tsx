import './db-refresh-logs-styles.scss';
import { FC } from 'react';
import { useUnmount } from 'react-use';
import { useSetAtom } from 'jotai';
import { FlexCol, FlexRow, Flexbox } from '@shared/components';
import { Grid, Typography } from '@mui/material';
import { resetPageStateAction } from '@store/dbRefreshLogs/dbRefreshLogsActions';
import DbLegendPopupButton from '@components/Site/DbLegend/DbLegendPopupButton';
import LogFilters from './Chunks/LogFiltersChunk';
import DbRefreshLogsGrid from './Grid/DbRefreshLogsGrid';

/** Страница логов БД. */
const DbRefreshLogsPage: FC = () => {
    const resetPage = useSetAtom(resetPageStateAction);

    useUnmount(resetPage);

    return (
        <FlexCol marginY="12px" flex="1" className="db-refresh-logs-page-container">
            <Grid container>
                <Grid size={{ xs: 12 }}>
                    <FlexRow alignItems="center" marginBottom={1} height={40}>
                        <Typography variant="subtitle1" fontWeight="bold"
                            marginLeft={2} marginRight={2}
                        >
                            Логи перезаливок БД
                        </Typography>
                        <Flexbox flex="1" flexDirection="row-reverse" height={40}>
                            <DbLegendPopupButton />
                            <LogFilters mr={2} />
                        </Flexbox>
                    </FlexRow>
                </Grid>
                <Grid size={{ xs: 12 }}>
                    <DbRefreshLogsGrid />
                </Grid>
            </Grid>
        </FlexCol>
    );
};

export default DbRefreshLogsPage;
