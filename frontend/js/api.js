const API_BASE = `${window.location.protocol}//${window.location.hostname}:53754/api`;

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

export async function fetchDishDetail(id) {
    const dishes = await fetchDishes();
    const dish = dishes.find(d => d.id.toString() === id.toString());
    if (!dish) throw new Error('Dish not found');
    return dish;
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

export async function fetchMyOrders() {
    const res = await fetch(`${API_BASE}/orders/my`);
    if (!res.ok) throw new Error('Failed to fetch your orders');
    return res.json();
}

export async function fetchOrderDetail(orderId) {
    const res = await fetch(`${API_BASE}/orders/${orderId}`);
    if (!res.ok) throw new Error('Failed to fetch order details');
    return res.json();
}

export async function fetchProfile() {
    const res = await fetch(`${API_BASE}/auth/profile`);
    if (!res.ok) throw new Error('Failed to fetch profile');
    return res.json();
}

export async function updateProfile(data) {
    const res = await fetch(`${API_BASE}/auth/profile`, {
        method: 'PUT', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error('Failed to update profile');
    return res.json();
}

export async function fetchChefProposals() {
    const res = await fetch(`${API_BASE}/chefs/proposals`);
    if (!res.ok) throw new Error('Failed to fetch menu proposals');
    return res.json();
}

export async function createProposal(data) {
    const res = await fetch(`${API_BASE}/chefs/proposals`, {
        method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error('Failed to create proposal');
    return res.json();
}

export async function updateProposal(id, data) {
    const res = await fetch(`${API_BASE}/chefs/proposals/${id}`, {
        method: 'PUT', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error('Failed to update proposal');
    return res.json();
}

export async function deleteProposal(id) {
    const res = await fetch(`${API_BASE}/chefs/proposals/${id}`, { method: 'DELETE' });
    if (!res.ok) throw new Error('Failed to delete proposal');
}

export async function fetchIncomingOrders() {
    const res = await fetch(`${API_BASE}/chefs/orders/incoming`);
    if (!res.ok) throw new Error('Failed to fetch incoming orders');
    return res.json();
}

export async function acceptOrder(id) {
    const res = await fetch(`${API_BASE}/chefs/orders/${id}/accept`, { method: 'POST' });
    if (!res.ok) throw new Error('Failed to accept order');
}

export async function rejectOrder(id) {
    const res = await fetch(`${API_BASE}/chefs/orders/${id}/reject`, { method: 'POST' });
    if (!res.ok) throw new Error('Failed to reject order');
}

export async function fetchDeliveries() {
    const res = await fetch(`${API_BASE}/chefs/deliveries`);
    if (!res.ok) throw new Error('Failed to fetch deliveries');
    return res.json();
}

export async function fetchDeliveryDetail(id) {
    const res = await fetch(`${API_BASE}/chefs/deliveries/${id}`);
    if (!res.ok) throw new Error('Failed to fetch delivery detail');
    return res.json();
}

export async function fetchChefProfile() {
    const res = await fetch(`${API_BASE}/auth/profile`); // reuse auth/profile
    if (!res.ok) throw new Error('Failed to fetch chef profile');
    return res.json();
}

export async function updateChefProfile(data) {
    const res = await fetch(`${API_BASE}/auth/profile`, {
        method: 'PUT', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error('Failed to update chef profile');
    return res.json();
}

export async function fetchStatsOrdersByChef() {
    const res = await fetch(`${API_BASE}/stats/orders-by-chef`);
    if (!res.ok) throw new Error('Failed to fetch stats orders by chef');
    return res.json();
}

export async function fetchStatsRevenueByStreet() {
    const res = await fetch(`${API_BASE}/stats/revenue-by-street`);
    if (!res.ok) throw new Error('Failed to fetch stats revenue by street');
    return res.json();
}

export async function fetchStatsAverageOrderPrice() {
    const res = await fetch(`${API_BASE}/stats/average-order-price`);
    if (!res.ok) throw new Error('Failed to fetch stats average order price');
    return res.json();
}

export async function fetchStatsTopCuisines() {
    const res = await fetch(`${API_BASE}/stats/top-cuisines`);
    if (!res.ok) throw new Error('Failed to fetch stats top cuisines');
    return res.json();
}
