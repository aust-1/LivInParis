// Customer-side interactions: browsing, cart, checkout
import {
    fetchDishes,
    placeOrder,
    fetchCustomerTransactions,
    fetchOrderDetail,
    fetchCustomerProfile,
    updateCustomerProfile,
    fetchDishDetail
} from './api.js';
import { showError, redirect, getCart, saveCart } from './common.js';


// Initialize a page based on its name
export function initPage(page) {
    switch (page) {
        case 'dashboard':
            updateCartCount();
            // Enable clicking on dashboard sub-nav links
            document.querySelectorAll('#content .sub-nav a').forEach(link => {
                link.addEventListener('click', e => {
                    e.preventDefault();
                    redirect(link.getAttribute('href'));
                });
            });
            break;
        case 'browse-dishes': initBrowse(); break;
        case 'cart': initCart(); break;
        case 'checkout': initCheckout(); break;
        case 'my-orders': initMyOrders(); break;
        case 'order-detail': initOrderDetail(); break;
        case 'dish-detail': initDishDetail(); break;
        case 'order-confirmation': initOrderConfirmation(); break;
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
    // View orders button on confirmation
    document.addEventListener('click', e => {
        if (e.target.id === 'btn-view-orders') redirect('#/customer/my-orders');
    });
}

async function initBrowse() {
    try {
        const listEl = document.getElementById('dishes-list');
        const dishes = await fetchDishes();
        listEl.innerHTML = '';
        dishes.forEach(d => {
            const card = document.createElement('div');
            card.className = 'dish-card';
            card.dataset.id = d.id;
            card.innerHTML = `
                <img src="${d.photoUrl || ''}" alt="${d.name}">
                <h3>${d.name}</h3>
                <p>€${d.price.toFixed(2)}</p>
                <button class="btn-add" data-id="${d.id}">+ Add to Cart</button>
                <button class="btn-details" data-id="${d.id}">Details</button>
            `;
            listEl.append(card);
        });
        listEl.addEventListener('click', e => {
            const id = e.target.dataset.id;
            if (e.target.classList.contains('btn-add')) {
                const cart = getCart();
                const existing = cart.find(i => i.id.toString() === id);
                if (existing) existing.quantity++;
                else {
                    const d = dishes.find(x => x.id.toString() === id);
                    cart.push({ id: d.id, name: d.name, price: d.price, quantity: 1 });
                }
                saveCart(cart);
                updateCartCount();
            }
            if (e.target.classList.contains('btn-details')) {
                sessionStorage.setItem('currentDishId', id);
                redirect('#/customer/dish-detail');
            }
        });
    } catch (err) {
        showError(err.message);
    }
}

function initCart() {
    const cart = getCart();
    const tbody = document.getElementById('cart-table').querySelector('tbody');
    tbody.innerHTML = '';
    cart.forEach(item => {
        const tr = document.createElement('tr');
        tr.dataset.id = item.id;
        tr.innerHTML = `
            <td>${item.name}</td>
            <td><input type="number" min="1" value="${item.quantity}" class="qty-input" data-id="${item.id}"></td>
            <td>€${item.price.toFixed(2)}</td>
            <td>€${(item.price * item.quantity).toFixed(2)}</td>
            <td><button class="btn-remove" data-id="${item.id}">Remove</button></td>
        `;
        tbody.append(tr);
    });
    updateCartTotal();
    tbody.addEventListener('click', e => {
        const id = e.target.dataset.id;
        if (e.target.classList.contains('btn-remove')) {
            const idx = cart.findIndex(i => i.id.toString() === id);
            if (idx >= 0) cart.splice(idx, 1);
            saveCart(cart);
            initCart();
            updateCartCount();
        }
    });
    tbody.addEventListener('change', e => {
        const id = e.target.dataset.id;
        if (e.target.classList.contains('qty-input')) {
            const item = cart.find(i => i.id.toString() === id);
            item.quantity = parseInt(e.target.value) || 1;
            saveCart(cart);
            initCart();
            updateCartCount();
        }
    });
    document.getElementById('btn-update-cart').addEventListener('click', () => initCart());
}

function updateCartTotal() {
    const cart = getCart();
    const total = cart.reduce((sum, i) => sum + i.price * i.quantity, 0);
    document.getElementById('cart-total').textContent = `€${total.toFixed(2)}`;
}

function updateCartCount() {
    const cart = getCart();
    document.getElementById('cart-count').textContent = cart.reduce((sum, i) => sum + i.quantity, 0);
}

async function initCheckout() {
    const form = document.getElementById('checkout-form');
    form.addEventListener('submit', async e => {
        e.preventDefault();
        const customerId = sessionStorage.getItem('accountId');
        if (!customerId) {
            showError('Missing customer id');
            return;
        }
        const payload = {
            addressId: parseInt(form.addressId?.value || form.address?.value || '0', 10)
        };
        try {
            const res = await placeOrder(customerId, payload);
            sessionStorage.setItem('confirmOrderId', res.id);
            saveCart([]);
            redirect('#/customer/order-confirmation');
        } catch (err) {
            showError(err.message);
        }
    });
}


async function initMyOrders() {
    try {
        const table = document.getElementById('orders-table').querySelector('tbody');
        const customerId = sessionStorage.getItem('accountId');
        if (!customerId) {
            showError('Missing customer id');
            return;
        }
        const orders = await fetchCustomerTransactions(customerId);
        table.innerHTML = '';
        orders.forEach(o => {
            const tr = document.createElement('tr');
            tr.dataset.id = o.id;
            tr.innerHTML = `
                <td>${o.id}</td>
                <td>${new Date().toLocaleString()}</td>
                <td>€${o.totalPrice.toFixed(2)}</td>
                <td>${o.lines?.[0]?.status || 'Pending'}</td>
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
        document.getElementById('order-detail-date').textContent = new Date().toLocaleString();
        document.getElementById('order-detail-status').textContent = detail.lines?.[0]?.status || 'Pending';
        const tbody = document.getElementById('order-detail-items').querySelector('tbody');
        tbody.innerHTML = '';
        detail.lines?.forEach(item => {
            const row = document.createElement('tr');
            row.innerHTML = `<td>${item.dishName}</td><td>1</td><td>€${item.unitPrice.toFixed(2)}</td>`;
            tbody.append(row);
        });
        document.getElementById('order-detail-total').textContent = `€${detail.totalPrice.toFixed(2)}`;
    } catch (err) {
        showError(err.message);
    }
}

async function initProfile() {
    try {
        const customerId = sessionStorage.getItem('accountId');
        if (!customerId) {
            showError('Missing customer id');
            return;
        }
        const prof = await fetchCustomerProfile(customerId);
        document.getElementById('profile-username').textContent = prof.username;
        document.getElementById('profile-email').textContent = prof.email;
        document.getElementById('profile-phone').textContent = prof.phoneNumber;
        document.getElementById('profile-address').textContent = `${prof.address?.number ?? ''} ${prof.address?.street ?? ''}`.trim();
    } catch (err) {
        showError(err.message);
    }
}

async function initEditProfile() {
    try {
        const form = document.getElementById('edit-profile-form');
        const customerId = sessionStorage.getItem('accountId');
        if (!customerId) {
            showError('Missing customer id');
            return;
        }
        const prof = await fetchCustomerProfile(customerId);
        form.firstName.value = prof.firstName || '';
        form.lastName.value = prof.lastName || '';
        form.email.value = prof.email || '';
        form.phone.value = prof.phoneNumber || '';
        form.addressNumber.value = prof.address?.number || '';
        form.street.value = prof.address?.street || '';
        form.addEventListener('submit', async e => {
            e.preventDefault();
            const data = {
                isCompany: prof.isCompany,
                firstName: form.firstName.value,
                lastName: form.lastName.value,
                email: form.email.value,
                phoneNumber: form.phone.value,
                address: {
                    number: parseInt(form.addressNumber.value, 10),
                    street: form.street.value
                }
            };
            try { await updateCustomerProfile(customerId, data); redirect('#/customer/profile'); }
            catch (err) { showError(err.message); }
        });
    } catch (err) { showError(err.message); }
}


async function initDishDetail() {
    try {
        const id = sessionStorage.getItem('currentDishId');
        const d = await fetchDishDetail(id);
        document.getElementById('dish-photo').src = d.photoUrl || '';
        document.getElementById('dish-name').textContent = d.name;
        document.getElementById('dish-type').textContent = `Type: ${d.type}`;
        document.getElementById('dish-price').textContent = `Price: €${d.price.toFixed(2)}`;
        document.getElementById('dish-expiry').textContent = d.expiryTime ?? '';
        document.getElementById('dish-nationality').textContent = `Cuisine: ${d.cuisine}`;
        document.getElementById('dish-ingredients').textContent = '';

        document.getElementById('btn-add-cart').addEventListener('click', () => {
            const cart = getCart();
            const existing = cart.find(i => i.id.toString() === id);
            if (existing) existing.quantity++;
            else cart.push({ id: d.id, name: d.name, price: d.price, quantity: 1 });
            saveCart(cart);
            updateCartCount();
        });
        document.getElementById('btn-buy-now').addEventListener('click', () => {
            const cart = getCart();
            const existing = cart.find(i => i.id.toString() === id);
            if (existing) existing.quantity++;
            else cart.push({ id: d.id, name: d.name, price: d.price, quantity: 1 });
            saveCart(cart);
            redirect('#/customer/checkout');
        });
        document.getElementById('btn-back-list').addEventListener('click', () => redirect('#/customer/browse-dishes'));
    } catch (err) { showError(err.message); }
}

function initOrderConfirmation() {
    const id = sessionStorage.getItem('confirmOrderId');
    const time = sessionStorage.getItem('confirmDeliveryTime');
    document.getElementById('confirm-order-id').textContent = id;
    document.getElementById('confirm-delivery-time').textContent = time;
}