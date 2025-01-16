/**
 * Объединение нескольких фильтров.
 * 
 * Example: const compositeFilter = ConnectFilters(MaxLength(10), OtherFilter)
 * @param filters
 */
export default function ConnectFilters<TValue>(
    ...filters: Array<(value: TValue) => TValue>
) {
    return (value: TValue) => {
        filters?.forEach(f => { value = f(value); });

        return value;
    };
}