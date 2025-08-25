import { FC } from 'react';
import {
    Button, Dialog, DialogActions, DialogContent, DialogTitle, FormGroup
} from '@mui/material';
import { AtomicTextField, AtomicNumberField } from '@shared/components';
import { confirmRefreshDelayMinutesState, confirmRefreshCommentState } from '@store/dbRefresh/dbRefreshState';

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
