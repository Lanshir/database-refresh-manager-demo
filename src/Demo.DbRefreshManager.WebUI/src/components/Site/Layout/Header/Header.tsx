import './header-styles.scss';
import { FC } from 'react';
import { Link } from 'react-router-dom';
import { useAtomValue } from 'jotai';
import {
    AppBar, Button, Stack, Toolbar, Typography, Link as MuiLink
} from '@mui/material';
import { Logo } from '@assets/svg';
import AccountMenu from './AccountMenu/AccountMenu';
import Routes from '@constants/routes';
import { authorizationState } from '@store/authorization/authorizationState';
import { configState } from '@store/config/configState';

/** Заголовок сайта. */
const Header: FC = () => {
    const { isAuthorized } = useAtomValue(authorizationState);
    const config = useAtomValue(configState);

    return (
        <AppBar className="app-header" position="sticky">
            <Toolbar>
                <img src={Logo} alt="Logo" className="app-logo" />
                <Typography variant="h5" fontWeight="bold">
                    Database Refresh Manager
                </Typography>

                {!isAuthorized ? null : (
                    <>
                        <Stack direction="row" ml={4}>
                            <Button variant="text" size="medium" color="inherit"
                                component={Link} to={Routes.home}
                            >
                                Список баз
                            </Button>
                            <Button variant="text" size="medium" color="inherit"
                                component={Link} to={Routes.dbRefreshLogs}
                            >
                                Логи перезаливок
                            </Button>
                            {/*
                            <Button variant="text" size="medium" color="inherit"
                                component={MuiLink} href={config.instructionUrl} target="_blank"
                            >
                                Инструкция
                            </Button>
                            */}
                        </Stack>

                        <AccountMenu />
                    </>
                )}
            </Toolbar>
        </AppBar>
    );
};

export default Header;
