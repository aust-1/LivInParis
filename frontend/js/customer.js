// Customer-side interactions: browsing, cart, checkout
import { fetchDishes, placeOrder, fetchMyOrders, fetchOrderDetail, fetchProfile, updateProfile } from './api.js';
import { showError, redirect } from './common.js';

// Initialize a page based on its name
export function initPage(page) {
    switch (page) {
        case 'dashboard': /* nothing to init */ break;
        case 'browse-dishes': initBrowse(); break;
        case 'cart': initCart(); break;
        case 'checkout': initCheckout(); break;
        case 'my-orders': initMyOrders(); break;
        case 'order-detail': initOrderDetail(); break;
        case 'profile': initProfile(); break;
        case 'edit-profile': initEditProfile(); break;
        // other pages don't require JS logic
    }
    // Navigation button handlers
    document.addEventListener('click', e => {
        const id = e.target.id;
        if (id === 'btn-back-dashboard') redirect('#/customer/dashboard');
        if (id === 'btn-back-orders') redirect('#/customer/my-orders');
        if (id === 'btn-back-list') redirect('#/customer/browse-dishes');
        if (id === 'btn-continue-shopping') redirect('#/customer/browse-dishes');
        if (id === 'btn-checkout') redirect('#/customer/checkout');
    });
}

async function initBrowse() {
    try {
        const list = document.getElementById('dishes-list');
        const dishes = await fetchDishes();
        dishes.forEach(d => {
            const item = document.createElement('div');
            item.textContent = d.name + ' – ' + d.price + '€';
            // Add button to add to cart...
            list.append(item);
        });
    } catch (err) {
        showError(err.message);
    }
}

function initCart() {
    // Load cart from sessionStorage, display, allow update/remove
}

function initCheckout() {
    const form = document.getElementById('checkout-form');
    form.addEventListener('submit', async e => {
        e.preventDefault();
        const order = {/* collect order details */ };
        try {
            await placeOrder(order);
            redirect('order-confirmation.html');
        } catch (err) {
            showError(err.message);
        }
    });
}

async function initMyOrders() {
    try {
        const table = document.getElementById('orders-table').querySelector('tbody');
        const orders = await fetchMyOrders();
        orders.forEach(o => {
            const tr = document.createElement('tr');
            tr.dataset.id = o.id;
            tr.innerHTML = `
                <td>${o.id}</td>
                <td>${new Date(o.date).toLocaleString()}</td>
                <td>€${o.total.toFixed(2)}</td>
                <td>${o.status}</td>
                <td><button class="btn-view-order" data-id="${o.id}">View</button></td>`;
            table.append(tr);
        });
        table.addEventListener('click', e => {
            if (e.target.classList.contains('btn-view-order')) {
                const id = e.target.dataset.id;
                sessionStorage.setItem('currentOrderId', id);
                redirect('#/customer/order-detail');
            }
        });
    } catch (err) {
        showError(err.message);
    }
}

async function initOrderDetail() {
    try {
        const id = sessionStorage.getItem('currentOrderId');
        const detail = await fetchOrderDetail(id);
        document.getElementById('order-detail-id').textContent = detail.id;
        document.getElementById('order-detail-date').textContent = new Date(detail.date).toLocaleString();
        document.getElementById('order-detail-status').textContent = detail.status;
        const tbody = document.getElementById('order-detail-items').querySelector('tbody');
        detail.items.forEach(item => {
            const row = document.createElement('tr');
            row.innerHTML = `<td>${item.dishName}</td><td>${item.quantity}</td><td>€${(item.subtotal).toFixed(2)}</td>`;
            tbody.append(row);
        });
        document.getElementById('order-detail-total').textContent = `€${detail.total.toFixed(2)}`;
    } catch (err) {
        showError(err.message);
    }
}

async function initProfile() {
    try {
        const profile = await fetchProfile();
        document.getElementById('profile-name').textContent = profile.name;
        document.getElementById('profile-username').textContent = profile.userName;
        document.getElementById('profile-email').textContent = profile.email;
        document.getElementById('profile-address').textContent = profile.address;
    } catch (err) {
        showError(err.message);
    }
}

async function initEditProfile() {
    try {
        const form = document.getElementById('edit-profile-form');
        const profile = await fetchProfile();
        form.name.value = profile.name;
        form.address.value = profile.address;
        form.addEventListener('submit', async e => {
            e.preventDefault();
            try {
                const data = { name: form.name.value, address: form.address.value };
                await updateProfile(data);
                redirect('#/customer/profile');
            } catch (err) {
                showError(err.message);
            }
        });
    } catch (err) {
        showError(err.message);
    }
}