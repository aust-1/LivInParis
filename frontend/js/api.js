// Wrapper around all HTTP calls to the .NET API
const API_BASE = 'http://localhost:5105/api';

export async function login(name, password) {
    const res = await fetch(`${API_BASE}/auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name, password })
    });
    if (!res.ok) throw new Error('Login failed');
    return res.json();
}

export async function register(data) {
    const res = await fetch(`${API_BASE}/auth/register`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error('Register failed');
    return res.json();
}

export async function getClients() {
    const res = await fetch(`${API_BASE}/clients`);
    if (!res.ok) throw new Error('Failed to fetch clients');
    return res.json();
}

export async function fetchDishes() {
    const res = await fetch(`${API_BASE}/dishes`);
    if (!res.ok) throw new Error('Failed to fetch dishes');
    return res.json();
}

export async function placeOrder(order) {
    const res = await fetch(`${API_BASE}/orders`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(order)
    });
    if (!res.ok) throw new Error('Order failed');
    return res.json();
}

export async function fetchProposals() {
    const res = await fetch(`${API_BASE}/chefs/proposals`);
    if (!res.ok) throw new Error('Failed to fetch proposals');
    return res.json();
}

export async function getRoute(fromId, toId) {
    const res = await fetch(`${API_BASE}/map/route?from=${fromId}&to=${toId}`);
    if (!res.ok) throw new Error('Failed to fetch route');
    return res.json();
}

// add other endpoints as needed