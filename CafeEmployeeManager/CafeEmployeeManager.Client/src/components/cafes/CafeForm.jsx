import React, { useEffect, useState, useRef } from 'react';
import { TextField, Button, Typography, Box } from '@mui/material';
import { useNavigate, useParams } from 'react-router-dom';
import { createCafe, getCafeById, updateCafe } from '../../services/cafeService';
import TemplatePage from '../TemplatePage';
const validateName = v => v && v.length >= 6 && v.length <= 10;
const validateDescription = v => v && v.length > 0 && v.length <= 256;
const validateLocation = v => v && v.length > 0 && v.length <= 10;

const CafeForm = () => {
    const [form, setForm] = useState({ name: '', description: '', location: '', logoUrl: '' });
    const [errors, setErrors] = useState({});
    const [dirty, setDirty] = useState(false);
    const navigate = useNavigate();
    const { id } = useParams();
    const fileInputRef = useRef(null);

    useEffect(() => {
        if (id) {
            getCafeById(id).then(data => {
                setForm({
                    name: data.name ?? '',
                    description: data.description ?? '',
                    location: data.location ?? '',
                    logoUrl: data.logo ?? ''
                });
            });
        }
    }, [id]);

    const onChange = e => {
        setForm(prev => ({ ...prev, [e.target.name]: e.target.value }));
        setDirty(true);
    };
    const validate = () => {
        const err = {};
        if (!validateName(form.name)) err.name = 'Name must be between 6 and 10 characters';
        if (!validateDescription(form.description)) err.description = 'Description must be between 6 and 10 characters';
        if (!validateLocation(form.location)) err.location = 'Locationmust be between 1 and 10 characters';
        setErrors(err);
        return Object.keys(err).length === 0;
    };
    const onFileChange = e => {
        const file = e.target.files[0];
        if (!file) return;
        const reader = new FileReader();
        reader.onload = () => setForm(prev => ({ ...prev, logoUrl: reader.result }));
        reader.readAsDataURL(file);
        setDirty(true);
    };

    const onSubmit = async e => {
        e.preventDefault();
        if (!validate()) return;
        const formData = new FormData();
        formData.append('Name', form.name);
        formData.append('Description', form.description);
        formData.append('Location', form.location);

        if (fileInputRef.current?.files[0]) {
            formData.append('LogoFile', fileInputRef.current.files[0]); // must match backend property
        }

        try {
            if (id) {
                await updateCafe(id, formData);
            } else {
                await createCafe(formData);
            }
            setDirty(false);
            navigate('/cafes');
        } catch {
            setErrors({ submit: 'Save failed. Please try again.' });
        }
    };

    const onCancel = () => {
        if (dirty && !window.confirm('Unsaved changes. Leave anyway?')) return;
        navigate('/cafes');
    };

    return (
        <TemplatePage title={id ? 'Edit Cafe' : 'Add Cafe'} error={errors.submit}>
            <form onSubmit={onSubmit}>
                <TextField label="Name" name="name" value={form.name} onChange={onChange}
                    error={!!errors.name}
                    helperText={errors.name} fullWidth margin="normal" />
                <TextField label="Description" name="description" value={form.description} onChange={onChange} error={!!errors.description}
                    helperText={errors.description} fullWidth margin="normal" multiline rows={3} />
                <TextField label="Location" name="location" value={form.location} onChange={onChange} error={!!errors.location}
                    helperText={errors.location} fullWidth margin="normal" />

                <Box sx={{ mt: 1 }}>
                    <input ref={fileInputRef} type="file" accept="image/*" onChange={onFileChange} />
                    {form.logoUrl && (
                        <Box sx={{ mt: 1 }}>
                            <img src={form.logoUrl} alt="logo" width={120} />
                            <Button variant="outlined" color="error" sx={{ mt: 1 }} onClick={() => { setForm(prev => ({ ...prev, logoUrl: '' })); if (fileInputRef.current) fileInputRef.current.value = ''; }}>
                                Remove
                            </Button>
                        </Box>
                    )}
                </Box>

                <Box sx={{ mt: 3, display: 'flex', gap: 2 }}>
                    <Button variant="contained" type="submit">Save</Button>
                    <Button variant="outlined" onClick={onCancel}>Cancel</Button>
                </Box>
            </form>
        </TemplatePage>
    );
};

export default CafeForm;
