import { forwardRef, ChangeEvent } from 'react';
import TextField, { TextFieldProps } from '@mui/material/TextField';
import { PrimitiveAtom, useAtom } from 'jotai';

export type AtomicTextFieldProps = Omit<TextFieldProps, 'value' | 'onChange'> & {
    stateAtom: PrimitiveAtom<string>
    setStateCallback?: (text: string) => void
};

/** TextField with atom input */
export const AtomicTextField = forwardRef<HTMLDivElement | null, AtomicTextFieldProps>(
    ({ children, stateAtom, setStateCallback, ...props }, ref) => {
        const [state, setState] = useAtom(stateAtom);

        const onChange = (event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => {
            const value = event.target.value;

            setState(value);

            if (!!setStateCallback) {
                setStateCallback(value);
            }
        };

        return <TextField ref={ref} {...props} value={state} onChange={onChange} />;
    }
);

export default AtomicTextField;
