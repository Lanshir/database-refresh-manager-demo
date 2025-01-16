/** Конфигурация frontend. */
export type FrontendConfig = {
    /** Url метода просмотра списка обновлённых объектов. */
    objectsListUrl: string
    /** Url интрукции к менеджеру. */
    instructionUrl: string
};

/** Массив ключей типа. */
export const FrontendConfigKeys: Readonly<Array<keyof FrontendConfig>> = [
    'objectsListUrl',
    'instructionUrl'
];

export default FrontendConfig;