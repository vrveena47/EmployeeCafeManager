import axios from 'axios';

const API_URL = 'https://localhost:7156/api/employee';


export const getEmployees = async (cafe = '') => {
    const res = await axios.get(`${API_URL}${cafe ? `?cafe=${encodeURIComponent(cafe)}` : ''}`);
    return res.data;
};

export const getEmployeeById = async (employeeId) => {
    const res = await axios.get(`${API_URL}/${employeeId}`);
    return res.data;
};

export const createEmployee = async (payload) => {
    // payload: { id, name, email_address, phone_number, gender, startDate, cafeId }  // Remove startDate from payload if present
    const { startDate, ...rest } = payload;
    const res = await axios.post(API_URL, rest);
    return res.data;
};

export const updateEmployee = async (employeeId, payload) => {
    // Remove startDate from payload if present
    const { startDate, ...rest } = payload;
    const res = await axios.put(`${API_URL}/${employeeId}`, rest);
    return res.data;
};

export const deleteEmployee = async (employeeId) => {
    const res = await axios.delete(`${API_URL}/${employeeId}`);
    return res.data;
};
