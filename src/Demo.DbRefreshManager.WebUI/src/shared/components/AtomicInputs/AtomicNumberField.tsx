import TextField, { TextFieldProps } from '@mui/material/TextField';
import { PrimitiveAtom, useAtom } from 'jotai';
import { ChangeEvent } from 'react';

export type AtomicNumberFieldProps = Omit<TextFieldProps, 'value' | 'onChange'> & {
    stateAtom: PrimitiveAtom<number>
};

const numberRegexp = /[^0-9]/ig;

/** TextField with number atom input */
export const AtomicNumberField = (
    { children, stateAtom, ref, ...props }: AtomicNumberFieldProps
) => {
    const [state, setState] = useAtom(stateAtom);

    const onChange = (event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => {
        const value = Number(event.target.value?.replaceAll(numberRegexp, ''));

        setState(value);
    };

    return <TextField ref={ref} {...props} value={state} onChange={onChange} />;
};

export default AtomicNumberField;
