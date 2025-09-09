import React, { useEffect, useState } from 'react';
import '../../utils/gridModules';
import { AgGridReact } from 'ag-grid-react';
import { Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { getEmployees, deleteEmployee } from '../../services/employeeService';
import ConfirmDialog from '../../shared/ConfirmDialog';
import TemplatePage from '../TemplatePage';

import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';

const EmployeeList = () => {
    const [employees, setEmployees] = useState([]);
    const [confirmOpen, setConfirmOpen] = useState(false);
    const [selected, setSelected] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        fetchEmployees();
    }, []);

    const fetchEmployees = async (cafe = '') => {
        try {
            const data = await getEmployees(cafe);
            setEmployees(Array.isArray(data) ? data : data?.employees || []);
        } catch (err) {
            console.error(err);
        }
    };

    const handleDelete = async () => {
        if (!selected) return;
        try {
            await deleteEmployee(selected.employeeId);
            setConfirmOpen(false);
            fetchEmployees();; 
        } catch (err) {
            console.error(err);
        }
    };

    const columns = [
        { headerName: 'Name', field: 'name' },
        { headerName: 'Email', field: 'emailAddress' },
        { headerName: 'Phone', field: 'phoneNumber' },
        { headerName: 'Days Worked', field: 'daysWorked' },
        { headerName: 'Cafe', field: 'cafe' },
        {
            headerName: 'Actions',
            field: 'actions',
            cellRenderer: params => (
                <>
                    <Button size="small" onClick={() => navigate(`/employees/edit/${params.data.employeeId}`)}>Edit</Button>
                    <Button size="small" color="error" onClick={() => { setSelected(params.data); setConfirmOpen(true); }}>Delete</Button>
                </>
            )
        }
    ];

    return (
        <TemplatePage
            title="Employees"
            actions={
                <Button variant="contained" color="primary" onClick={() => navigate('/employees/create')}>
                    Add New Employee
                </Button>
            }
        >
            <div className="ag-theme-alpine" style={{ height: '65vh', width: '100%' }}>
                <AgGridReact rowData={employees} columnDefs={columns} rowSelection="single" theme="legacy" />
            </div>
            <ConfirmDialog
                open={confirmOpen}
                onClose={() => setConfirmOpen(false)}
                onConfirm={handleDelete}
                message={`Are you sure you want to delete employee "${selected?.name}"?`}
            />
        </TemplatePage>
    );
};

export default EmployeeList;
