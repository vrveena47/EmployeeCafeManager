import axios from 'axios';

const API_URL = 'https://localhost:7156/api/cafe';

export const getCafes = async (location = '') => {
    const response = await axios.get(`${API_URL}?location=${location}`);
    return response.data;
};

export const getCafeById = async (id) => {
    const response = await axios.get(`${API_URL}/${id}`);
    return response.data;
};

export const createCafe = async (cafe) => {
    const response = await axios.post(API_URL, cafe, {
        headers: { 'Content-Type': 'application/form-data' }
    });
    return response.data;
};


export const updateCafe = async (id, cafe) => {
    const response = await axios.put(`${API_URL}/${id}`, cafe ,{
        headers: { 'Content-Type': 'application/form-data' }
    });
    return response.data;
};

export const deleteCafe = async (id) => {
    const response = await axios.delete(`${API_URL}/${id}`);
    return response.data;
};
