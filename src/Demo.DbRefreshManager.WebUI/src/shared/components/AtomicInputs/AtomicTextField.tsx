import TextField, { TextFieldProps } from '@mui/material/TextField';
import { PrimitiveAtom, useAtom } from 'jotai';
import { ChangeEvent } from 'react';

export type AtomicTextFieldProps = Omit<TextFieldProps, 'value' | 'onChange'> & {
    stateAtom: PrimitiveAtom<string>
};

/** TextField with atom input */
export const AtomicTextField = (
    { children, stateAtom, ref, ...props }: AtomicTextFieldProps
) => {
    const [state, setState] = useAtom(stateAtom);

    const onChange = (event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => {
        const value = event.target.value;

        setState(value);
    };

    return <TextField ref={ref} {...props} value={state} onChange={onChange} />;
};

export default AtomicTextField;
