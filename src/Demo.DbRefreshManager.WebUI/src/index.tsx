import '@shared/styles/sass/site.scss';
import 'dayjs/locale/ru';
import { createRoot } from 'react-dom/client';
import dayjs from 'dayjs';
import dayjsUtc from 'dayjs/plugin/utc';
import App from './App';

// Установка языка day js.
dayjs.locale('ru');
dayjs.extend(dayjsUtc);

const container = document.getElementById('root');
const root = createRoot(container!);

root.render(<App />);
