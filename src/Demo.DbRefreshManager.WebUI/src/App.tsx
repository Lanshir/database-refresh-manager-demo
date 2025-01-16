import { FC, PropsWithChildren } from 'react';
import { Provider } from 'jotai';
import { ApolloProvider } from '@apollo/client';
import LightTheme from '@infrastructure/muiThemes/themeLight';
import SnackbarAlertRoot from '@components/SnackbarAlert/SnackbarAlertRoot';
import { ApolloClient } from '@apiClients';
import { StyledEngineProvider, ThemeProvider } from '@mui/material';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';

const App: FC<PropsWithChildren> = ({ children }) => {
    return (
        <Provider>
            <ApolloProvider client={ApolloClient}>
                { /* App css priority over MUI css */}
                <StyledEngineProvider injectFirst>
                    { /* App MUI theme */}
                    <ThemeProvider theme={LightTheme}>
                        { /* Mui datepicker adapter */}
                        <LocalizationProvider dateAdapter={AdapterDayjs}>
                            <SnackbarAlertRoot>
                                {children}
                            </SnackbarAlertRoot>
                        </LocalizationProvider>
                    </ThemeProvider>
                </StyledEngineProvider>
            </ApolloProvider>
        </Provider>
    );
};

export default App;
