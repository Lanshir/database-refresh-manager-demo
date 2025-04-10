import React from 'react';
import { Box, BoxProps } from '@mui/material';

interface IFlexColProps extends Omit<BoxProps, 'display' | 'flexDirection'> {
    //
}

/**
 * Обёртка для Box с display="flex" flexDirection="column".
 */
export const FlexCol = React.forwardRef<unknown, IFlexColProps>(({ children, ...props }, ref) =>
    <Box ref={ref} {...props}
        // display overrides default hidden property.
        display={!props.hidden ? 'flex' : 'none'}
        flexDirection="column"
    >
        {children}
    </Box>);

export default FlexCol;