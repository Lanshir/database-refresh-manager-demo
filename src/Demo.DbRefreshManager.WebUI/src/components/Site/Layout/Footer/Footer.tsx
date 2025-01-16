import './footer-styles.scss';
import { FC, useEffect, useRef, useState } from 'react';
import { Container, Typography } from '@mui/material';
import { Flexbox } from '@shared/components';
import dayjs from 'dayjs';

/**
 * Подвал приложения.
 */
const Footer: FC = () => {
    const refreshIntervalRef = useRef<NodeJS.Timeout | null>(null);
    const [today, setToday] = useState(dayjs());

    // Эффект обновления даты в футере.
    useEffect(() => {
        if (!!refreshIntervalRef.current) {
            clearInterval(refreshIntervalRef.current!);
        }

        const currentTodayDay = today.toDate().getDay();

        refreshIntervalRef.current = setInterval(() => {
            if (currentTodayDay !== new Date().getDay()) {
                setToday(dayjs());
            }
        }, 1000);

        return () => { clearInterval(refreshIntervalRef.current!); };
    }, [today]);

    return (
        <Flexbox className="app-footer"
            sx={{
                backgroundColor: theme => theme.palette.primary.dark,
                color: theme => theme.palette.common.white
            }}
        >
            <Container className="container">
                <Typography>
                    {`Сегодня ${today.format('dddd DD.MM.YYYY')}`}
                </Typography>
            </Container>
        </Flexbox>
    );
};

export default Footer;