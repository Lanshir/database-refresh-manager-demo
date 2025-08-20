import { FC } from 'react';
import {
    Button, Dialog, DialogActions, DialogContent, DialogTitle, Typography, SxProps
} from '@mui/material';

export type ConfirmDialogProps = {
    open: boolean;
    title: string;
    text: string;
    buttons: Array<'Yes' | 'No' | 'Cancel'>;
    onYes?: () => void;
    onNo?: () => void;
    onCancel?: () => void;
    onClose(): void;
}

const buttonSx = { width: 80 } as SxProps;

/** Диалоговое окно подтверждения действия. */
export const ConfirmDialog: FC<ConfirmDialogProps> = (props) => {
    const { open, title, text, buttons, onYes, onNo, onCancel, onClose } = props;

    return (
        <Dialog open={open} onClose={onClose}>
            <DialogTitle>{title}</DialogTitle>

            <DialogContent>
                <Typography variant="body1">
                    {text}
                </Typography>
            </DialogContent>

            <DialogActions>
                {!buttons.some(b => b === 'Cancel') ? null :
                    <Button sx={buttonSx} variant="text" onClick={onCancel} >
                        Отмена
                    </Button>
                }
                {!buttons.some(b => b === 'No') ? null :
                    <Button sx={buttonSx} variant="outlined" onClick={onNo} >
                        Нет
                    </Button>
                }
                {!buttons.some(b => b === 'Yes') ? null :
                    <Button sx={buttonSx} variant="contained" onClick={onYes} >
                        Да
                    </Button>
                }
            </DialogActions>
        </Dialog>
    );
};

export default ConfirmDialog;