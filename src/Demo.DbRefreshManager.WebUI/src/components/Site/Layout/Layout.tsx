import './layout-styles.scss';
import { FC } from 'react';
import { Container } from '@mui/material';
import { FlexCol } from '@shared/components';
import { Outlet } from 'react-router-dom';
import AuthChecker from '@components/AuthChecker/AuthChecker';
import Header from './Header/Header';
import Footer from './Footer/Footer';

const Layout: FC = () => {
    return (
        <FlexCol flex="1" className="app-layout">
            <Header />
            <Container className="app-container">
                <AuthChecker>
                    <Outlet />
                </AuthChecker>
            </Container>
            <Footer />
        </FlexCol>
    );
};

export default Layout;
