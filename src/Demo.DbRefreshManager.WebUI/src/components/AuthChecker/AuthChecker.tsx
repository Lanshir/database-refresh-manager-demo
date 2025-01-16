import { FC, PropsWithChildren, useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router';
import { useSetAtom } from 'jotai';
import { checkAuthQuery } from '@store/authorization/authorizationActions';
import { LoadingBackdrop } from '@shared/components';

/**
 * Компонент проверки активной авторизации. 
 */
const AuthChecker: FC<PropsWithChildren> = ({ children }) =>
{
    const checkAuth = useSetAtom(checkAuthQuery);
    const [authChecked, setAuthChecked] = useState(false);

    const location = useLocation();
    const navigate = useNavigate();

    useEffect(() => {
        if (!authChecked) {
            checkAuth(navigate, location).then(() => setAuthChecked(true));
        }
    }, [checkAuth, location, navigate, authChecked]);

    return authChecked ? children : <LoadingBackdrop open backdropOpacity="0.1" />;
};

export default AuthChecker;