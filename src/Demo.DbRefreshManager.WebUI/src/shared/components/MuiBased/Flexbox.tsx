import React from 'react';
import { Box, BoxProps } from '@mui/material';

interface IFlexboxProps extends Omit<BoxProps, 'display'> {
    //
}

/**
 * Обёртка для Box с display="flex".
 */
export const Flexbox = React.forwardRef<unknown, IFlexboxProps>(({ children, ...props }, ref) =>
    <Box ref={ref} {...props}
        // display overrides default hidden property.
        display={!props.hidden ? 'flex' : 'none'}
    >
        {children}
    </Box>
);

export default Flexbox;