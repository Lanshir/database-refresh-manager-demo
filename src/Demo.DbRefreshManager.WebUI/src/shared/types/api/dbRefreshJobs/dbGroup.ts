/**
 * Пункт группы БД. 
 */
export type DbGroup = {
    id: number,
    sortOrder: number,
    description: string,
    cssColor: string
};

/** Массив ключей типа. */
export const DbGroupKeys: Readonly<Array<keyof DbGroup>> = [
    'id',
    'sortOrder',
    'description',
    'cssColor'
];

export default DbGroup;