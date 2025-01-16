import { FC, PropsWithChildren } from 'react';
import {
    SnackbarAction, SnackbarOrigin, SnackbarProvider,
    VariantType, closeSnackbar
} from 'notistack';
import { IconButton } from '@mui/material';
import { CheckCircle, Close, Error, Info, Warning } from '@mui/icons-material';
import SnackbarToStoreBind from './SnackbarToStoreBind';
import './snackbar-styles.scss';

/**
 * Провайдер оповещений в Snackbar. 
 */
const SnackbarAlertRoot: FC<PropsWithChildren> = ({ children }) =>
    <SnackbarProvider maxSnack={5}
        autoHideDuration={10000}
        anchorOrigin={anchorOrigin}
        iconVariant={icons}
        action={dismissAction}
    >
        <SnackbarToStoreBind />
        {children}
    </SnackbarProvider>;

const anchorOrigin: SnackbarOrigin = {
    horizontal: 'right',
    vertical: 'bottom'
};

const icons: Partial<Record<VariantType, React.ReactNode>> = {
    error: <Error className="snack-icon" />,
    success: <CheckCircle className="snack-icon" />,
    warning: <Warning className="snack-icon" />,
    info: <Info className="snack-icon" />
};

const dismissAction: SnackbarAction = (snackId) =>
    <IconButton onClick={() => closeSnackbar(snackId)}>
        <Close sx={{ color: theme => theme.palette.common.white }} />
    </IconButton>;

export default SnackbarAlertRoot;