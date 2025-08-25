import { atom } from 'jotai';
import { loadable } from 'jotai/utils';
import FrontendConfig from '@shared/types/api/frontendConfig/frontendConfig';
import { GetConfig } from '@requests/graphql/queries/frontendConfigQueries';

/** Конфигурация frontend. */
export const configState = atom<FrontendConfig>({
    objectsListUrl: '',
    instructionUrl: ''
});

/** Асинхронное состояние конфигурациии frontend. */
export const configAsyncState = loadable(atom(
    async () => await GetConfig() as FrontendConfig
));
