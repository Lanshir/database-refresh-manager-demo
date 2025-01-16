import { KeyboardEvent } from 'react';

/**
 * Хук обработчика клавиатурных событий OnKeyPress, OnKeyDown итд...
 * @param predicate Ф-я - условие.
 * @param handler Обработчик.
 */
export function useKeyboardEventHandler<TElement = Element>(
    predicate: (event: KeyboardEvent<TElement>) => boolean,
    handler: (event: KeyboardEvent<TElement>) => void
): typeof onKbEvent {

    const onKbEvent = (event: KeyboardEvent<TElement>) => {
        if (predicate(event)) {
            handler(event);
        }
    };

    return onKbEvent;
}

export default useKeyboardEventHandler;