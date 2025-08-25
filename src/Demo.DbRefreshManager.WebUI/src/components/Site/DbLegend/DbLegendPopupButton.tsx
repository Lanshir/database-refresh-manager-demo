import './db-legend-popup-styles.scss';
import { FC, useCallback, useMemo, useState } from 'react';
import { DensitySmall } from '@mui/icons-material';
import { Box, Button, Popover } from '@mui/material';
import DbLegend from './DbLegend';

/**
 * Кнопка меню легенды БД.
 */
const DbLegendPopupButton: FC = () => {
    const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);

    const openPopover = useCallback(
        (e: React.MouseEvent<HTMLElement>) => setAnchorEl(e.currentTarget), []);

    const closePopover = useCallback(() => setAnchorEl(null), []);

    return (
        <>
            <Button variant="outlined"
                endIcon={<DensitySmall />}
                onClick={openPopover}
            >
                Легенда
            </Button>
            <Popover className="db-legend-popup"
                anchorEl={anchorEl}
                open={!!anchorEl}
                onClose={closePopover}
                anchorOrigin={useMemo(() => ({
                    vertical: 'bottom',
                    horizontal: 'right'
                }), [])}
                transformOrigin={useMemo(() => ({
                    vertical: 'top',
                    horizontal: 'right'
                }), [])}
            >
                <Box className="legend-popup-content-wrap">
                    <DbLegend />
                </Box>
            </Popover>
        </>
    );
};

export default DbLegendPopupButton;
