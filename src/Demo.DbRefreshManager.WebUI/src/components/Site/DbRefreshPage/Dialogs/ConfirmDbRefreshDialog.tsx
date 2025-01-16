import { FC } from 'react';
import {
    Button, Dialog, DialogActions, DialogContent, DialogTitle,
    FormGroup, TextField
} from '@mui/material';
import { useJotaiNumberInputState, useJotaiTextInputState } from '@hooks';
import { confirmRefreshInputPropState } from '@store/dbRefresh/dbRefreshState';

interface DialogProps {
    open: boolean
    title: string
    onYes: () => void
    onNo: () => void
}

/** Диалоговое окно подтверждения перезаливки БД. */
const ConfirmDbRefreshDialog: FC<DialogProps> =
    ({ open, title, onYes, onNo }) => {
        const [delayMinutes, , OnChangeDelayMinutes] =
            useJotaiNumberInputState(confirmRefreshInputPropState('delayMinutes'));

        const [comment, , onCommentChange] =
            useJotaiTextInputState(confirmRefreshInputPropState('comment'));

        return (
            <Dialog fullWidth maxWidth="xs" open={open} onClose={onNo}>
                <DialogTitle>{title}</DialogTitle>

                <DialogContent>
                    <FormGroup>
                        <TextField label="Минут до запуска"
                            size="small" margin="dense"
                            value={delayMinutes}
                            onChange={OnChangeDelayMinutes}
                        />
                        <TextField label="Комментарий"
                            multiline rows={3}
                            size="small" margin="dense"
                            value={comment}
                            onChange={onCommentChange}
                        />
                    </FormGroup>
                </DialogContent>

                <DialogActions>
                    <Button sx={{ width: 100 }} variant="outlined" onClick={onNo} >
                        Нет
                    </Button>
                    <Button sx={{ width: 100 }} variant="contained" onClick={onYes} >
                        Да
                    </Button>
                </DialogActions>
            </Dialog>
        );
    };

export default ConfirmDbRefreshDialog;