

export async function getClients() {
    const res = await fetch(`${API}/clients`);
    return res.json();
}

export async function createClient(client) {
    await fetch(`${API}/clients`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(client)
    });
}

//TODO: idem pour chefs, dishes, orders…

const API = "http://localhost:5105/api";

export async function login(email, password) {
    const res = await fetch(`${API}/auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
    });
    if (!res.ok) throw new Error('Login failed');
    return res.json();
}

export async function register(data) {
    const res = await fetch(`${API}/auth/register`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error('Register failed');
    return res.json();
}

// Customer endpoints
export async function fetchDishes() {
    const res = await fetch(`${API}/dishes`);
    return res.json();
}

export async function placeOrder(order) {
    const res = await fetch(`${API}/orders`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(order)
    });
    if (!res.ok) throw new Error('Order failed');
    return res.json();
}

// Chef endpoints
export async function fetchProposals() {
    const res = await fetch(`${API}/chefs/proposals`);
    return res.json();
}

// Map & routing
export async function getRoute(fromId, toId) {
    const res = await fetch(`${API}/map/route?from=${fromId}&to=${toId}`);
    if (!res.ok) throw new Error('Failed to fetch route');
    return res.json();
}

// etc.
