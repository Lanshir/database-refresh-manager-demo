import dayjs from 'dayjs';

/*
 * Функции валидации.
 */

/**
 * Поле обязательно к заполнению.
 * @returns Текст ошибки.
 */
export function TextRequired(text?: string, error = 'Поле не должно быть пустым') {
    text = text?.trim() ?? '';

    return text.length > 0 ? '' : error;
}

/**
 * Поле с числовым значением обязательно к заполнению.
 * @returns Текст ошибки.
 */
export function NumberRequired(num?: number | null, error = 'Поле не должно быть пустым') {
    return typeof num === 'number' ? '' : error;
}

/**
 * Требуется корректная дата.
 * @returns Текст ошибки.
 */
export function DateRequired(date?: Date | null, error = 'Укажите дату') {
    return (!date || isNaN(date.valueOf())) ? error : '';
}

/**
 * Требуется корректная строка даты.
 * @returns Текст ошибки.
 */
export function DateStringRequired(date?: string, error = 'Укажите дату') {
    return (!date || !dayjs(date).isValid()) ? error : '';
}

/**
 * Строка минимальной длины.
 * @returns Текст ошибки.
 */
export function MinLength(
    text?: string,
    minLength: number = 1,
    error = `Введите минимум ${minLength} символов`
) {
    text = text?.trim() ?? '';

    return text.length >= minLength ? '' : error;
}

/**
 * Строка минимальной длины.
 * @returns Текст ошибки.
 */
export function MaxLength(
    text?: string,
    maxLength: number = 1,
    error = `Введите минимум ${maxLength} символов`
) {
    text = text?.trim() ?? '';

    return text.length <= maxLength ? '' : error;
}

/**
 * Строка точной длины.
 * @returns Текст ошибки.
 */
export function ExactLength(
    text?: string,
    length: number = 1,
    error = `Введите ${length} символов`
) {
    text = text?.trim() ?? '';

    return text.length === length ? '' : error;
}

/**
 * Текст является Email адресом.
 * @returns Текст ошибки.
 */
export function IsEmail(text?: string, error = 'Введите корректный email') {
    text = text?.trim() ?? '';

    const regex = new RegExp('^[а-яА-ЯёЁa-zA-Z0-9.!#$%&\'*+/=?^_`{|}~-]+@[а-яА-ЯёЁa-zA-Z0-9]+[.]{1}[а-яА-ЯёЁa-zA-Z]+$');

    return regex.test(text) ? '' : error;
}

/**
 * Текст является телефоном.
 * @returns Текст ошибки.
 */
export function IsPhone(text?: string, error = 'Введите телефон') {
    const regex = new RegExp('[^0-9]', 'g');

    text = text?.replace(regex, '')?.slice(-11);

    return text?.length === 11 ? '' : error;
}
