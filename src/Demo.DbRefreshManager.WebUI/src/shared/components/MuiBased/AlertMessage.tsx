import { FC } from 'react';
import { Alert, AlertColor, Collapse, CollapseProps } from '@mui/material';

export type AlertMessageProps = CollapseProps & {
    open?: boolean
    severity?: AlertColor
    variant?: 'filled' | 'outlined' | 'standard'
    title?: string
    text?: string
    onClose?: () => void
};

/** Сворачиваемый Alert с сообщением. */
export const AlertMessage: FC<AlertMessageProps> = (props) => {
    const { open, severity, variant, title, text, onClose, children, sx, ...other } = props;

    return (
        <Collapse in={open} {...other}>
            <Alert sx={sx} severity={severity} variant={variant}
                title={title}
                onClose={onClose}
            >
                {text}
            </Alert>
        </Collapse>
    );
};

export default AlertMessage;
