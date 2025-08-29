import { FC } from 'react';
import { BrowserRouter } from 'react-router-dom';
import { ApolloProvider } from '@apollo/client';
import { ApolloClient } from '@apiClients';
import LightTheme from '@infrastructure/muiThemes/themeLight';
import { StyledEngineProvider, ThemeProvider } from '@mui/material';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import ApiClientsConfigurer from '@components/Configurations/ApiClientsConfigurer';
import SnackbarAlertRoot from '@components/SnackbarAlert/SnackbarAlertRoot';
import ConfigLoader from '@components/Configurations/ConfigLoader';
import Router from '@components/Router/Router';

const App: FC = () => {
    return (
        <BrowserRouter>
            <ApiClientsConfigurer>
                <ApolloProvider client={ApolloClient}>
                    { /* App css priority over MUI css */}
                    <StyledEngineProvider injectFirst>
                        { /* App MUI theme */}
                        <ThemeProvider theme={LightTheme}>
                            { /* Mui datepicker adapter */}
                            <LocalizationProvider dateAdapter={AdapterDayjs}>
                                <SnackbarAlertRoot>
                                    <ConfigLoader />
                                    <Router />
                                </SnackbarAlertRoot>
                            </LocalizationProvider>
                        </ThemeProvider>
                    </StyledEngineProvider>
                </ApolloProvider>
            </ApiClientsConfigurer>
        </BrowserRouter>
    );
};

export default App;
