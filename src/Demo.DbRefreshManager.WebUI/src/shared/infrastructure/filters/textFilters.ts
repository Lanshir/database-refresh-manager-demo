/*
 * Фильтры текста.
 */

/** Максимальная длина */
export const MaxLength = (length: number) => (text: string) => {
    return text.length > length ? text.substring(0, length) : text;
};

/**
 * Исключение символов подходящих под регулярку.
 * Прим.:
 * - оставить только цифры и анг. буквы [^0-9a-zA-Z];
 * - убрать все цифры [0-9].
 * @param regexp Регулярка.
 */
export const RegexpExclude = (regexp: string) => (text: string) => {
    const regex = new RegExp(regexp, 'g');

    return text.replace(regex, '');
};

/** Допустимые символы номера телефона. */
export const PhoneChars = (text: string) => {
    const regex = new RegExp('[^0-9()+-]', 'g');

    return text.replace(regex, '');
};
