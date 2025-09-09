import React from 'react';
import { TextField } from '@mui/material';

const TextInput = ({ label, name, value, onChange, error, helperText, ...rest }) => (
    <TextField
        fullWidth
        label={label}
        name={name}
        value={value}
        onChange={onChange}
        error={!!error}
        helperText={helperText || (error ? error : '')}
        {...rest}
    />
);

export default TextInput;
