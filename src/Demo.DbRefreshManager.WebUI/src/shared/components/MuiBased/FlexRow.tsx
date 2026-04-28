import { Box, BoxProps } from '@mui/material';

export type FlexRowProps = Omit<BoxProps, 'display' | 'flexDirection'> & {
    //
};

/**
 * Обёртка для Box с display="flex" flexDirection="row".
 */
export const FlexRow = ({ children, ref, ...props }: FlexRowProps) => (
    <Box ref={ref} {...props}
        // display overrides default hidden property.
        display={!props.hidden ? 'flex' : 'none'}
        flexDirection="row"
    >
        {children}
    </Box>
);

export default FlexRow;
