import { FC, useEffect } from 'react';
import { useAtomValue, useSetAtom } from 'jotai';
import { configState, configAsyncState } from '@store/config/configState';

/**
 * Загрузчик конфигурации frontend.
 */
const ConfigLoader: FC = () => {
    const configLoadable = useAtomValue(configAsyncState);
    const setConfig = useSetAtom(configState);

    useEffect(() => {
        switch (configLoadable.state) {
            case 'hasData':
                setConfig(configLoadable.data);
                break;
            case 'hasError':
                console.error(
                    'Ошибка загрузки конфигурации frontend',
                    configLoadable.error);
                break;
        }
    }, [configLoadable, setConfig]);

    return null;
};

export default ConfigLoader;
