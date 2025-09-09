import React from 'react';
import { Dialog, DialogActions, DialogContent, DialogContentText, Button } from '@mui/material';

const ConfirmDialog = ({ open, onClose, onConfirm, message }) => (
    <Dialog open={open} onClose={onClose}>
        <DialogContent>
            <DialogContentText>{message}</DialogContentText>
        </DialogContent>
        <DialogActions>
            <Button onClick={onClose} color="primary">Cancel</Button>
            <Button onClick={onConfirm} color="secondary">Confirm</Button>
        </DialogActions>
    </Dialog>
);

export default ConfirmDialog;
