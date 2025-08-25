import { CSSInterpolation, createTheme } from '@mui/material/styles';
import { ruRU as muiRu } from '@mui/material/locale';
import { ruRU as gridRu } from '@mui/x-data-grid/locales';
import { ruRU as dateRu } from '@mui/x-date-pickers/locales';

/**
 * Глобальные параметры тем MUI.
 */

/**
 * NOTE: доступ к классу элемента, не определённому в объекте темы.
 *
 * root: {
 *    '&.Mui-class': <CssInterpolation>{
 *        // some style.
 *    }
 * }
 *
 * */

/** Общие параметры тем. */
const themeGlobal = createTheme({
    components: {
        MuiContainer: {
            defaultProps: {
                maxWidth: false
            }
        },
        MuiDataGrid: {
            styleOverrides: {
                // Перенос строки в заголовках.
                columnHeaderTitle: {
                    whiteSpace: 'break-spaces',
                    lineHeight: 1.2
                },
                row: {
                    // Ячейка с прокруткой большого текста для строк фиксированной высоты
                    // Исп. - добавить cellClassName: rich-text-cell в объявление колонки.
                    '&:not(.MuiDataGrid-row--dynamicHeight)>.MuiDataGrid-cell.rich-text-cell': <CSSInterpolation>{
                        overflowY: 'auto',
                        alignItems: 'flex-start',
                        '& .MuiDataGrid-cellContent': <CSSInterpolation>{
                            display: 'flex',
                            minHeight: '100%',
                            alignItems: 'center',
                            whiteSpace: 'pre-line'
                        }
                    },
                    '&:not(.MuiDataGrid-row--dynamicHeight)>.MuiDataGrid-cell.pre-line-text': <CSSInterpolation>{
                        whiteSpace: 'pre-line'
                    }
                },
                cell: {
                    alignContent: 'center',

                    '&.pre-line-text': <CSSInterpolation>{
                        whiteSpace: 'pre-line'
                    }
                }
            }
        }
    }
}, muiRu, gridRu, dateRu);

export default themeGlobal;
