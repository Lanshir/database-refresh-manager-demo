import Routes from '@constants/routes';
import { MaxLength } from '@filters';
import { useCheckboxState, useKeyboardEventHandler, useTextInputState } from '@hooks';
import { Visibility, VisibilityOff } from '@mui/icons-material';
import {
    Alert, Button, Card, CardActions, CardContent, Checkbox, FormControlLabel,
    IconButton, InputAdornment, TextField, Typography
} from '@mui/material';
import { AlertMessage, FlexCol } from '@shared/components';
import { authorizeQuery } from '@store/authorization/authorizationActions';
import {
    alertState, authorizationState, loginErrorsState, loginLoadingState
} from '@store/authorization/authorizationState';
import { useAtom, useAtomValue, useSetAtom } from 'jotai';
import { useResetAtom } from 'jotai/utils';
import { FC, useEffect, useState } from 'react';
import { useNavigate } from 'react-router';
import { useMount, useUnmount } from 'react-use';
import './login-styles.scss';

/**
 * Форма авторизации.
 */
const Login: FC = () => {
    const { isAuthorized } = useAtomValue(authorizationState);
    const isLoading = useAtomValue(loginLoadingState);
    const errors = useAtomValue(loginErrorsState);
    const resetErrors = useResetAtom(loginErrorsState);
    const resetAuth = useResetAtom(authorizationState);

    const [alert, setAlert] = useAtom(alertState);
    const [login, , onLoginChange] = useTextInputState('', MaxLength(200), resetErrors);
    const [password, , onPasswordChange] = useTextInputState('', MaxLength(200), resetErrors);
    const [rememberMe, , onRememberMeChange] = useCheckboxState(true);
    const [isPassVisible, setPassVisible] = useState(false);

    const authorize = useSetAtom(authorizeQuery);
    const navigate = useNavigate();

    const onLogin = () => authorize(login, password, rememberMe);

    const onTogglePassClick = () => setPassVisible(!isPassVisible);
    const onEnterKeyUp = useKeyboardEventHandler(e => e.key === 'Enter', onLogin);

    // Сброс авторизации при редиректе по 401 коду.
    useMount(resetAuth);

    // Unmount cleanup.
    useUnmount(resetErrors);

    // Переадресация если пользователь авторизован.
    useEffect(() => {
        if (isAuthorized) {
            navigate({ pathname: Routes.home });
        }
    }, [navigate, isAuthorized]);

    return (
        <FlexCol className="login-container" onKeyUp={onEnterKeyUp}>
            <Card className="login-card">
                <CardContent className="card-content">
                    <Typography variant="h6" marginBottom={1}>
                        Авторизация
                    </Typography>

                    <AlertMessage open={!!alert.text}
                        sx={{ mb: 2 }}
                        severity={alert.severity}
                        text={alert.text}
                        onClose={() => setAlert({ text: '' })}
                    />

                    <Alert sx={{ mb: 1 }} severity="info">
                        Demo users logins:
                        <br />
                        demoMaster, demoAnalyst, demoSupport
                        <br />
                        <br />
                        Password: pwd
                    </Alert>

                    <TextField label="Логин" fullWidth
                        size="small" margin="dense"
                        value={login}
                        error={!!errors.login}
                        helperText={errors.login ?? ''}
                        onChange={onLoginChange}
                        disabled={isLoading}
                    />

                    <TextField label="Пароль" fullWidth
                        size="small" margin="dense"
                        value={password}
                        error={!!errors.password}
                        helperText={errors.password ?? ''}
                        onChange={onPasswordChange}
                        disabled={isLoading}
                        type={isPassVisible ? 'text' : 'password'}
                        // Toggle button.
                        slotProps={{
                            input: {
                                endAdornment: (
                                    <InputAdornment position="end">
                                        <IconButton onClick={onTogglePassClick}>
                                            {isPassVisible ? <Visibility /> : <VisibilityOff />}
                                        </IconButton>
                                    </InputAdornment>
                                )

                            }
                        }}
                    />

                    <FormControlLabel label="Запомнить меня"
                        labelPlacement="start"
                        sx={{ userSelect: 'none' }}
                        control={(
                            <Checkbox sx={{ paddingY: 0 }}
                                checked={rememberMe}
                                onChange={onRememberMeChange}
                                disabled={isLoading}
                            />
                        )}
                    />
                </CardContent>
                <CardActions className="card-actions">
                    <Button variant="contained" size="medium" fullWidth
                        onClick={onLogin}
                        disabled={isLoading}
                    >
                        Вход
                    </Button>
                </CardActions>
            </Card>
        </FlexCol>
    );
};

export default Login;
