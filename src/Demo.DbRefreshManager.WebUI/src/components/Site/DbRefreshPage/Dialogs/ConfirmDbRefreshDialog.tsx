import {
    Button, Dialog, DialogActions, DialogContent, DialogTitle, FormGroup
} from '@mui/material';
import { AtomicNumberField, AtomicTextField } from '@shared/components';
import { confirmRefreshCommentState, confirmRefreshDelayMinutesState } from '@store/dbRefresh/dbRefreshState';
import { FC } from 'react';

type DialogProps = {
    open: boolean
    title: string
    onYes: () => void
    onNo: () => void
};

/** Диалоговое окно подтверждения перезаливки БД. */
const ConfirmDbRefreshDialog: FC<DialogProps>
    = ({ open, title, onYes, onNo }) => (
        <Dialog fullWidth maxWidth="xs" open={open} onClose={onNo}>
            <DialogTitle>{title}</DialogTitle>

            <DialogContent>
                <FormGroup>
                    <AtomicNumberField stateAtom={confirmRefreshDelayMinutesState}
                        label="Минут до запуска" size="small" margin="dense"
                    />
                    <AtomicTextField stateAtom={confirmRefreshCommentState}
                        label="Комментарий" size="small" margin="dense"
                        multiline rows={3}
                    />
                </FormGroup>
            </DialogContent>

            <DialogActions>
                <Button sx={{ width: 100 }} variant="outlined" onClick={onNo}>
                    Нет
                </Button>
                <Button sx={{ width: 100 }} variant="contained" onClick={onYes}>
                    Да
                </Button>
            </DialogActions>
        </Dialog>
    );

export default ConfirmDbRefreshDialog;
