import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import NavBar from './components/NavBar';
import CafeList from './components/cafes/CafeList';
import CafeForm from './components/cafes/CafeForm';
import EmployeeList from './components/employees/EmployeeList';
import EmployeeForm from './components/employees/EmployeeForm';

export default function App() {
    return (
        <>
            <NavBar />
            <Routes>
                <Route path="/" element={<Navigate to="/cafes" replace />} />
                <Route path="/cafes" element={<CafeList />} />
                <Route path="/cafes/create" element={<CafeForm />} />
                <Route path="/cafes/edit/:id" element={<CafeForm />} />
                <Route path="/employees" element={<EmployeeList />} />
                <Route path="/employees/create" element={<EmployeeForm />} />
                <Route path="/employees/edit/:id" element={<EmployeeForm />} />
                <Route path="*" element={<div>Not Found</div>} />
            </Routes>
        </>
    );
}

