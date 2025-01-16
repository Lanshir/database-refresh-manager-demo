import './db-refresh-page-styles.scss';
import { FC } from 'react';
import { useUnmount } from 'react-use';
import { useSetAtom } from 'jotai';
import { Alert, Grid2 as Grid, Typography } from '@mui/material';
import { FlexCol, FlexRow, Flexbox } from '@shared/components';
import { resetPageStateAction } from '@store/dbRefresh/dbRefreshActions';
import DbLegendPopupButton from '@components/Site/DbLegend/DbLegendPopupButton';
import DbListFilters from './Chunks/DbListFiltersChunk';
import DbJobsListGrid from './Grid/DbJobsListGrid';
import PageDialogs from './Dialogs/PageDialogs';

/** Страница перезаливки БД */
const DbRefreshPage: FC = () => {
    const resetState = useSetAtom(resetPageStateAction);

    // Unmount cleanup.
    useUnmount(resetState);

    return (
        <FlexCol marginY="12px" flex="1" className="db-refresh-page-container">
            <PageDialogs />

            <Grid container>
                <Grid size={{ xs: 12 }}>
                    <FlexRow alignItems="center" marginBottom={1} height={40}>
                        <Typography variant="subtitle1" fontWeight="bold"
                            marginLeft={2} marginRight={2}
                        >
                            Список баз на перезаливку
                        </Typography>
                        <Alert severity="info" className="refresh-times-alert">
                            Базы нужно перезаливать не реже чем раз в 3 дня.
                        </Alert>
                        <Flexbox flex="1" flexDirection="row-reverse" height={40}>
                            <DbLegendPopupButton />
                            <DbListFilters />
                        </Flexbox>
                    </FlexRow>
                </Grid>
                <Grid size={{ xs: 12 }}>
                    <DbJobsListGrid />
                </Grid>
            </Grid>
        </FlexCol>
    );
};

export default DbRefreshPage;