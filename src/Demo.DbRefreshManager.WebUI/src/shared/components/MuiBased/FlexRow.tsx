import React from 'react';
import { Box, BoxProps } from '@mui/material';

interface IFlexRowProps extends Omit<BoxProps, 'display' | 'flexDirection'> {
    //
}

/**
 * Обёртка для Box с display="flex" flexDirection="row".
 */
export const FlexRow = React.forwardRef<unknown, IFlexRowProps>(({ children, ...props }, ref) =>
    <Box ref={ref} {...props}
        // display overrides default hidden property.
        display={!props.hidden ? 'flex' : 'none'}
        flexDirection="row"
    >
        {children}
    </Box>);

export default FlexRow;