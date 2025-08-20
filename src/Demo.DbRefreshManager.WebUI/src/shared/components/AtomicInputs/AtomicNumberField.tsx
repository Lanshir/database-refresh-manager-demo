import { forwardRef, ChangeEvent } from 'react';
import TextField, { TextFieldProps } from '@mui/material/TextField';
import { PrimitiveAtom, useAtom } from 'jotai';

export type AtomicNumberFieldProps = Omit<TextFieldProps, 'value' |'onChange'> & {
    stateAtom: PrimitiveAtom<number>,
    setStateCallback?: (num: number) => void
}

const numberRegexp = /[^0-9]/ig;

/** TextField with number atom input */
export const AtomicNumberField = forwardRef<HTMLDivElement | null, AtomicNumberFieldProps>(
    ({ children, stateAtom, setStateCallback, ...props }, ref) => {
        const [state, setState] = useAtom(stateAtom);

        const onChange = (event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => {
            const value = Number(event.target.value?.replaceAll(numberRegexp, ''));

            setState(value);

            if (!!setStateCallback) {
                setStateCallback(value);
            }
        };

        return <TextField ref={ref} {...props} value={state} onChange={onChange} />
    }
);

export default AtomicNumberField;