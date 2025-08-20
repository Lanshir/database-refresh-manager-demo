import { FC } from 'react';
import { Backdrop, BackdropProps, CircularProgress } from '@mui/material';

export type LoadingBackdropProps = BackdropProps & {
    backdropOpacity?: string
}

/** Фоновая анимация загрузки. */
export const LoadingBackdrop: FC<LoadingBackdropProps> = (props) => {
    const { children, backdropOpacity, ...other } = props;

    return (
        <Backdrop
            sx={{
                color: 'white',
                zIndex: 2000,
                backgroundColor: `rgba(0, 0, 0, ${backdropOpacity ?? '0.3'})`
            }}
            {...other}
        >
            <CircularProgress color="inherit" />
        </Backdrop>
    );
};

export default LoadingBackdrop;