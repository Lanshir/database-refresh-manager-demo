import { FC } from 'react';
import { useMount } from 'react-use';
import { useAtomValue, useSetAtom } from 'jotai';
import { Box, Skeleton, Stack, Typography } from '@mui/material';
import { legendLoadingState, dbLegendState } from '@store/dbLegend/dbLegendState';
import { dbLegendQuery } from '@store/dbLegend/dbLegendActions';

/**
 * Легенда БД. 
 */
const DbLegend: FC = () => {
    const isLoading = useAtomValue(legendLoadingState);
    const legend = useAtomValue(dbLegendState);
    const loadLegend = useSetAtom(dbLegendQuery);

    useMount(() => {
        if (!isLoading && legend.length === 0) {
            loadLegend();
        }
    });

    return (
        <Stack direction="column" spacing={1}>
            {
                isLoading
                    ? [...Array(10)].map((v, i) =>
                        <Skeleton key={`legend-skeleton-${i}`}
                            variant="rectangular"
                            height={32}
                        />)
                    : legend.map((v, i) =>
                        <Box key={`legend-item-${v.id}`} sx={{
                            backgroundColor: v.cssColor,
                            border: '1px solid lightgrey',
                            boxShadow: '-2px 1px 6px 0 lightgrey',
                            padding: '4px 8px'
                        }}>
                            <Typography>{v.description}</Typography>
                        </Box>)
            }
        </Stack>
    );
};

export default DbLegend;