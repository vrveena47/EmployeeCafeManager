import React, { useEffect, useState } from 'react';
import '../../utils/gridModules';
import { AgGridReact } from 'ag-grid-react';
//import { themeQuartz } from 'ag-grid-community';
import { Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { getCafes, deleteCafe } from '../../services/cafeService';
import ConfirmDialog from '../../shared/ConfirmDialog';
import TemplatePage from '../TemplatePage';

import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';

const CafeList = () => {
    const [cafes, setCafes] = useState([]);
    const [confirmOpen, setConfirmOpen] = useState(false);
    const [selectedCafe, setSelectedCafe] = useState(null);
    const navigate = useNavigate();
   
    useEffect(() => {
        getCafes('').then(setCafes);
    }, []);

    const columns = [
        {
            headerName: 'Logo',
            field: 'logo',
            cellRenderer: params =>
                params.value ? <img src={params.value} alt="logo" width={50} /> : null
        },
        { headerName: 'Name', field: 'name' },
        { headerName: 'Description', field: 'description' },
        { headerName: 'Employees', field: 'employees' },
        { headerName: 'Location', field: 'location' },
        {
            headerName: 'Actions',
            field: 'actions',
            cellRenderer: params => (
                <>
                    <Button onClick={() => navigate(`/cafes/edit/${params.data.cafeId}`)}>Edit</Button>
                    <Button
                        color="secondary"
                        onClick={() => {
                            setSelectedCafe(params.data);
                            setConfirmOpen(true);
                        }}
                    >
                        Delete
                    </Button>
                </>
            )
        }
    ];

    return (
        <TemplatePage
            title="Cafe List"
            actions={
                <Button variant="contained" onClick={() => navigate('/cafes/create')}>
                    Add New Cafe
                </Button>
            }
        >
            <div className="ag-theme-alpine" style={{ height: '70vh', width: '100%' }}>
                <AgGridReact rowData={cafes} columnDefs={columns} rowSelection="single" theme="legacy" />
            </div>
            <ConfirmDialog
                open={confirmOpen}
                onClose={() => setConfirmOpen(false)}
                onConfirm={async () => {
                        await deleteCafe(selectedCafe.cafeId); setConfirmOpen(false); getCafes('').then(setCafes); }}
                message={`Are you sure you want to delete cafe "${selectedCafe?.name}"?`}
            />
        </TemplatePage>
    );
};

export default CafeList;
