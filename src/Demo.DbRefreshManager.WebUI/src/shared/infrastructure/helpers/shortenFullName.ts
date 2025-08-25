/**
 * Укорачивание строки Фамилия Имя Отчество.
 */
export default function ShortenFullName(fullName?: string) {
    const shortName = (fullName ?? '')
        .split(' ')
        .map((name, i) => i === 0 ? `${name} ` : `${name.substring(0, 1)}.`)
        .join('');

    return shortName;
}
