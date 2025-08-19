import globalTheme from './themeGlobal';
import { createTheme, ThemeOptions } from '@mui/material/styles';
import { CSSInterpolation } from '@mui/system';
import type { } from '@mui/x-data-grid/themeAugmentation';

/**
 * Светлая тема MUI.
 */

// Predefined palette to acces when configure components.
let lightTheme = createTheme(globalTheme, <ThemeOptions>{
    palette: { mode: 'light' }
});

// Further overrides.
lightTheme = createTheme(lightTheme, <ThemeOptions>{
    components: {
        MuiOutlinedInput: {
            styleOverrides: {
                root: {
                    backgroundColor: lightTheme.palette.common.white,
                    '&.Mui-disabled': <CSSInterpolation>{
                        backgroundColor: lightTheme.palette.action.disabledBackground,
                        opacity: 0.4
                    }
                }
            }
        },
        MuiDataGrid: {
            styleOverrides: {
                columnHeader: {
                    color: lightTheme.palette.common.white,
                    backgroundColor: lightTheme.palette.primary.dark
                },
                main: {
                    backgroundColor: lightTheme.palette.common.white
                }
            }
        }
    }
});

export default lightTheme;