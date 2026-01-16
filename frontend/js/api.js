const API_BASE = `${window.location.protocol}//${window.location.host}/api`;

const API_BASE = `${window.location.protocol}//${window.location.host}/api`;

async function request(path, options = {}) {
    const res = await fetch(`${API_BASE}${path}`, {
        headers: { 'Content-Type': 'application/json', ...(options.headers || {}) },
        ...options
    });
    if (!res.ok) {
        const message = await res.text();
        throw new Error(message || `Request failed: ${res.status}`);
    }
    if (res.status === 204) return null;
    return res.json();
}


export async function login(username, password) {
    return request('/auth/login', {
        method: 'POST',
        body: JSON.stringify({ username, password })
    });
}

export async function register(data) {
    return request('/auth/register', {
        method: 'POST',
        body: JSON.stringify(data)
    });
}

export async function fetchDishes() {
    return request('/dishes');
}

export async function fetchDishDetail(id) {
    return request(`/dishes/${id}`);
}

export async function placeOrder(customerId, payload) {
    return request(`/checkout/customers/${customerId}`, {
        method: 'POST',
        body: JSON.stringify(payload)
    });
}

export async function fetchChefProposals(chefId) {
    return request(`/chefs/${chefId}/proposals`);
}

export async function createProposal(chefId, data) {
    return request(`/chefs/${chefId}/proposals`, {
        method: 'POST',
        body: JSON.stringify(data)
    });
}

export async function deleteProposal(chefId, proposalDate) {
    return request(`/chefs/${chefId}/proposals/${proposalDate}`, {
        method: 'DELETE'
    });
}

export async function fetchIncomingOrders(chefId) {
    return request(`/chefs/${chefId}/orders`);
}

export async function acceptOrder(chefId, orderId) {
    return request(`/chefs/${chefId}/orders/${orderId}/accept`, { method: 'POST' });
}

export async function rejectOrder(chefId, orderId) {
    return request(`/chefs/${chefId}/orders/${orderId}/reject`, { method: 'POST' });
}

export async function updateOrderStatus(chefId, orderId, status) {
    return request(`/chefs/${chefId}/orders/${orderId}/status`, {
        method: 'PUT',
        body: JSON.stringify({ status })
    });
}

export async function fetchCustomerTransactions(customerId) {
    return request(`/transaction/customers/${customerId}`);
}

export async function fetchOrderDetail(orderId) {
    return request(`/transaction/${orderId}`);
}

export async function fetchCustomerProfile(customerId) {
    return request(`/customers/${customerId}/profile`);
}

export async function updateCustomerProfile(customerId, data) {
    return request(`/customers/${customerId}/profile`, {
        method: 'PUT',
        body: JSON.stringify(data)
    });
}

export async function fetchChefProfile(chefId) {
    return request(`/chefs/${chefId}/profile`);
}

export async function updateChefProfile(chefId, data) {
    return request(`/chefs/${chefId}/profile`, {
        method: 'PUT',
        body: JSON.stringify(data)
    });
}

export async function fetchStatsOrdersByChef() {
    return request('/statistics/chef-deliveries');
}

export async function fetchStatsRevenueByStreet() {
    return request('/statistics/revenue-by-street');
}

export async function fetchStatsAverageOrderPrice() {
    return request('/statistics/average-order-price');
}

export async function fetchStatsTopCuisines() {
    return request('/statistics/top-cuisines');
}

export async function getRoute(fromAddress, toAddress) {
    return request(`/graph/route?fromAddress=${encodeURIComponent(fromAddress)}&toAddress=${encodeURIComponent(toAddress)}`);
}

export async function fetchReviews(accountId, reviewerType, rating) {
    const params = new URLSearchParams({ accountId, reviewerType });
    if (rating !== undefined && rating !== null) params.append('rating', rating);
    return request(`/review?${params.toString()}`);
}

export async function createReview(data) {
    return request('/review', {
        method: 'POST',
        body: JSON.stringify(data)
    });
}

