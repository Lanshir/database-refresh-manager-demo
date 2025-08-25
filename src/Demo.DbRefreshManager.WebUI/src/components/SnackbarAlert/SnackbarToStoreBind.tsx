import { FC, useEffect } from 'react';
import { useAtom } from 'jotai';
import { popAlertAction } from '@store/alerts/alertsActions';
import { useSnackbar } from 'notistack';

/**
 * Логика привязки вывода ошибок из store в notistack.
 */
const SnackbarToStoreBind: FC = () => {
    const { enqueueSnackbar } = useSnackbar();
    const [alerts, popAlert] = useAtom(popAlertAction);

    useEffect(() => {
        const lastAlert = popAlert();

        if (!!lastAlert) {
            enqueueSnackbar(lastAlert.message, { variant: lastAlert.variant });
        }
    }, [enqueueSnackbar, alerts, popAlert]);

    return null;
};

export default SnackbarToStoreBind;
