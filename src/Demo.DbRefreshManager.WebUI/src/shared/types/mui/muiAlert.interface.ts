import { AlertColor } from '@mui/material';

/**
 * Alert сообщение.
 */
interface IAlert {
    severity?: AlertColor;
    text?: string;
}

export default IAlert;