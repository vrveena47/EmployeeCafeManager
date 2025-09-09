import axios from 'axios'

const API_BASE = import.meta.env.VITE_API_BASE || 'http://localhost:5000'
const client = axios.create({ baseURL: API_BASE, timeout: 10000 })

// Cafes
export async function getCafes(location) {
    const res = await client.get('/cafes', { params: location ? { location } : {} })
    return res.data
}

export async function createCafe(payload) {
    // payload can be FormData or JSON
    if (payload instanceof FormData) {
        return (await client.post('/cafe', payload, { headers: { 'Content-Type': 'multipart/form-data' } })).data
    }
    return (await client.post('/cafe', payload)).data
}

export async function updateCafe(payload) {
    if (payload instanceof FormData) {
        return (await client.put('/cafe', payload, { headers: { 'Content-Type': 'multipart/form-data' } })).data
    }
    return (await client.put('/cafe', payload)).data
}

export async function deleteCafe(id) {
    return (await client.delete('/cafe', { data: { id } })).data
}

// Employees
export async function getEmployees(cafe) {
    const res = await client.get('/employees', { params: cafe ? { cafe } : {} })
    return res.data
}

export async function createEmployee(payload) {
    return (await client.post('/employee', payload)).data
}

export async function updateEmployee(payload) {
    return (await client.put('/employee', payload)).data
}

export async function deleteEmployee(id) {
    return (await client.delete('/employee', { data: { id } })).data
}
