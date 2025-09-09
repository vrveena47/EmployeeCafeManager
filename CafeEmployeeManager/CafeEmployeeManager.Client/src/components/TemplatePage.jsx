import React from 'react';
import { Box, Typography, Alert } from '@mui/material';

const TemplatePage = ({ title, actions, children, error }) => (
    <Box
        sx={{
            width: '100vw',
            minHeight: '100vh',
            p: { xs: 2, sm: 3, md: 6 },
            bgcolor: '#f5f5f5',
            boxSizing: 'border-box',
            display: 'flex',
            flexDirection: 'column',
        }}
    >
        {/* Header */}
        <Box sx={{ display: 'flex', alignItems: 'center', mb: 3 }}>
            <Typography variant="h4" sx={{ flex: 1 }}>
                {title}
            </Typography>
            {actions}
        </Box>

        {/* Error Alert */}
        {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
                {error}
            </Alert>
        )}

        {/* Content */}
        <Box
            sx={{
                flex: 1,
                display: 'flex',
                flexDirection: 'column',
                bgcolor: 'white',
                borderRadius: 2,
                boxShadow: 3,
                p: 2,
            }}
        >
            {children}
        </Box>
    </Box>
);

export default TemplatePage;
