import { FC, Fragment } from 'react';
import { Typography } from '@mui/material';

/** Page not found component. */
const NotFound: FC = () => {
    return (
        <Fragment>
            <Typography textAlign="center" variant="h4" mt="15%">
                Страница не найдена
            </Typography>
            <Typography textAlign="center" variant="subtitle1" mt="16px">
                Вы перешли по неправильной ссылке
            </Typography>
        </Fragment>
    );
};

export default NotFound;
