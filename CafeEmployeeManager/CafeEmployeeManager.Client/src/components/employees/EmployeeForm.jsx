import React, { useEffect, useState } from 'react';
import { TextField, Button, Typography, RadioGroup, FormControlLabel, Radio, MenuItem, Box } from '@mui/material';
import { useNavigate, useParams } from 'react-router-dom';
import { getEmployees, getEmployeeById, createEmployee, updateEmployee } from '../../services/employeeService';
import { getCafes } from '../../services/cafeService';
import TemplatePage from '../TemplatePage';

const validateName = v =>  /^[A-Za-z]{6,10}$/.test(v);
const validateEmail = v => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v);
const validatePhone = v => /^[89]\d{7}$/.test(v);

const EmployeeForm = () => {
    const [form, setForm] = useState({ name: '', emailAddress: '', phoneNumber: '', gender: 'Male', cafeId: '' });
    const [cafes, setCafes] = useState([]);
    const [errors, setErrors] = useState({});
    const [dirty, setDirty] = useState(false);
    const navigate = useNavigate();
    const { id } = useParams();

    useEffect(() => {
        (async () => {
            const cafesData = await getCafes('');
            setCafes(cafesData || []);
        })();
    }, []);

    useEffect(() => {
        if (id) {
            (async () => {
                const data = await getEmployeeById(id);
                setForm({
                    name: data.name || '',
                    emailAddress: data.emailAddress || '',
                    phoneNumber: data.phoneNumber || '',
                    gender: data.gender || 'Male',
                    cafeId: data.cafeId ? String(data.cafeId) : ''
                }); console.log('Form after load:', { cafe: data.cafeId  })
            })();
        }
    }, [id]);

    useEffect(() => {
        const handler = (e) => {
            if (dirty) {
                e.preventDefault();
                e.returnValue = '';
            }
        };
        window.addEventListener('beforeunload', handler);
        return () => window.removeEventListener('beforeunload', handler);
    }, [dirty]);

    const onChange = (e) => {

        //let value = e.target.value;

        //// Convert empty string to null for cafeId
        //if (e.target.name === 'cafeId' && value === '') value = null;
        setForm(prev => ({ ...prev, [e.target.name]: e.target.value }));
        setDirty(true);
    };
   
    const validate = () => {
        const err = {};
        if (!validateName(form.name)) err.name = 'Name must be between 6 and 10 characters';
        if (!validateEmail(form.emailAddress)) err.emailAddress = 'Invalid email';
        if (!validatePhone(form.phoneNumber)) err.phoneNumber = 'Invalid SG phone (8 or 9 + 7 digits)';
        setErrors(err);
        return Object.keys(err).length === 0;
    };
    

    const onSubmit = async (e) => {
        e.preventDefault();
        if (!validate()) return;
        const payload = {
            ...form,
            cafeId: form.cafeId === '' ? null : form.cafeId
        };
        try {
            if (id) {
                await updateEmployee(id, payload);
            } else {
                await createEmployee(payload);
            }
            setDirty(false);
            navigate('/employees');
        } catch (err) {
            setErrors({ submit: 'Save failed. Please try again.' });
        }
    };

    const cancel = () => {
        if (dirty && !window.confirm('You have unsaved changes. Leave anyway?')) return;
        navigate('/employees');
    };

    return (
        <TemplatePage title={id ? 'Edit Employee' : 'Add New Employee'} error={errors.submit}>
            <form onSubmit={onSubmit}>
                {/*{id && (*/}
                {/*    <TextField*/}
                {/*        label="Employee ID (UIXXXXXXX)"*/}
                {/*        name="id"*/}
                {/*        value={form.id}*/}
                {/*        fullWidth*/}
                {/*        margin="normal"*/}
                {/*        disabled*/}
                {/*        helperText="Format UIXXXXXXX"*/}
                {/*    />*/}
               {/* {{ id }}*/}

                <TextField
                    label="Name"
                    name="name"
                    value={form.name}
                    onChange={onChange}
                    error={!!errors.name}
                    helperText={errors.name}
                    fullWidth
                    margin="normal"
                />

                <TextField
                    label="Email"
                    name="emailAddress"
                    value={form.emailAddress}
                    onChange={onChange}
                    error={!!errors.emailAddress}
                    helperText={errors.emailAddress}
                    fullWidth
                    margin="normal"
                />

                <TextField
                    label="Phone"
                    name="phoneNumber"
                    value={form.phoneNumber}
                    onChange={onChange}
                    error={!!errors.phoneNumber}
                    helperText={errors.phoneNumber}
                    fullWidth
                    margin="normal"
                />

                <Box sx={{ mt: 2 }}>
                    <Typography variant="subtitle2">Gender</Typography>
                    <RadioGroup row name="gender" value={form.gender} onChange={onChange}>
                        <FormControlLabel value="Male" control={<Radio />} label="Male" />
                        <FormControlLabel value="Female" control={<Radio />} label="Female" />
                    </RadioGroup>
                </Box>

                <TextField
                    select
                    label="Assigned Cafe"
                    name="cafeId"
                    value={form.cafeId ?? ''} // bind null as empty string
                    onChange={onChange}
                    fullWidth
                    margin="normal"
                >
                    <MenuItem value="">Not assigned</MenuItem>
                    {cafes.map(c => (<MenuItem key={c.cafeId} value={String(c.cafeId)}>{c.name}</MenuItem>))}
                </TextField>
               
                <Box sx={{ mt: 3, display: 'flex', gap: 2 }}>
                    <Button variant="contained" type="submit">Save</Button>
                    <Button variant="outlined" onClick={cancel}>Cancel</Button>
                </Box>
            </form>
        </TemplatePage>
    );
};

export default EmployeeForm;
