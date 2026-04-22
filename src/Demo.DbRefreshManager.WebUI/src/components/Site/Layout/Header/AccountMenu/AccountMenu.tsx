import Routes from '@constants/routes';
import { ShortenFullName } from '@helpers';
import { Button, Typography } from '@mui/material';
import { FlexRow } from '@shared/components';
import { deauthorizeQuery } from '@store/authorization/authorizationActions';
import { authorizationState } from '@store/authorization/authorizationState';
import { useAtomValue, useSetAtom } from 'jotai';
import { FC } from 'react';
import { useNavigate } from 'react-router';

/**
 * Блок меню авторизованного пользователя.
 */
const AccountMenu: FC = () => {
    const { user } = useAtomValue(authorizationState);
    const deauthorize = useSetAtom(deauthorizeQuery);
    const navigate = useNavigate();

    const onDeauthorize = () => {
        deauthorize(() => navigate({ pathname: Routes.login }));
    };

    return (
        <FlexRow alignItems="center" marginLeft="auto">
            <Typography marginRight={2}>
                {ShortenFullName(user.fullName)}
            </Typography>
            <Button variant="contained" color="secondary"
                onClick={onDeauthorize}
            >
                Выйти
            </Button>
        </FlexRow>
    );
};

export default AccountMenu;
