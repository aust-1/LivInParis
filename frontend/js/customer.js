import { fetchDishes, placeOrder, fetchMyOrders, fetchOrderDetail, fetchCustomerProfile, updateProfile, fetchDishDetail } from './api.js';
import { showError, redirect, getCart, saveCart } from './common.js';

export function initPage(page) {
    switch (page) {
        case 'dashboard':
            updateCartCount();
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
        case 'customer-profile': initProfile(); break;
        case 'edit-profile': initEditProfile(); break;
    }
    document.addEventListener('click', e => {
        const id = e.target.id;
        if (id === 'btn-back-dashboard') redirect('#/customer/dashboard');
        if (id === 'btn-back-orders') redirect('#/customer/my-orders');
        if (id === 'btn-back-list') redirect('#/customer/browse-dishes');
        if (id === 'btn-continue-shopping') redirect('#/customer/browse-dishes');
        if (id === 'btn-checkout') redirect('#/customer/checkout');
    });
    document.addEventListener('click', e => {
        if (e.target.id === 'btn-view-orders') redirect('#/customer/my-orders');
    });
}

async function initBrowse() {
    const listEl = document.getElementById('dishes-list');
    const filterBtn = document.getElementById('btn-filter');

    async function applyFilters() {
        try {
            const filters = {
                name: document.getElementById('filter-name').value.trim(),
                type: document.getElementById('filter-type').value,
                origin: document.getElementById('filter-origin').value,
                vegetarian: document.getElementById('filter-vegetarian').checked,
                vegan: document.getElementById('filter-vegan').checked,
                glutenFree: document.getElementById('filter-glutenfree').checked,
                lactoseFree: document.getElementById('filter-lactosefree').checked,
                halal: document.getElementById('filter-halal').checked,
                kosher: document.getElementById('filter-kosher').checked,
                priceMin: parseFloat(document.getElementById('filter-price-min').value) || null,
                priceMax: parseFloat(document.getElementById('filter-price-max').value) || null
            };

            const dishes = await fetchDishes(filters);
            listEl.innerHTML = '';

            if (dishes.length === 0) {
                listEl.innerHTML = '<p>No dishes found matching your criteria.</p>';
                return;
            }

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
        } catch (err) {
            showError(err.message);
        }
    }

    filterBtn.addEventListener('click', applyFilters);
    await applyFilters();
    listEl.addEventListener('click', e => {
        const id = e.target.dataset.id;
        if (e.target.classList.contains('btn-add')) {
            const cart = getCart();
            const existing = cart.find(i => i.id.toString() === id);
            if (existing) existing.quantity++;
            else {
                const d = [...listEl.children].find(x => x.dataset.id === id);
                if (!d) return;
                const name = d.querySelector('h3').textContent;
                const price = parseFloat(d.querySelector('p').textContent.replace('€', ''));
                cart.push({ id: Number(id), name, price, quantity: 1 });
            }
            saveCart(cart);
            updateCartCount();
        }
        if (e.target.classList.contains('btn-details')) {
            sessionStorage.setItem('currentDishId', id);
            redirect('#/customer/dish-detail');
        }
    });
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
        const order = {
            items: getCart(),
            address: form.address.value,
            paymentMethod: form.payment.value
        };
        try {
            const res = await placeOrder(order);
            sessionStorage.setItem('confirmOrderId', res.id);
            sessionStorage.setItem('confirmDeliveryTime', res.estimatedDelivery);
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
        const customerId = sessionStorage.getItem("customerId");
        const profile = await fetchCustomerProfile(customerId);

        document.getElementById('profile-name').textContent = `${profile.firstName} ${profile.lastName}`;
        document.getElementById('profile-username').textContent = profile.username;
        document.getElementById('profile-email').textContent = profile.email;
        document.getElementById('profile-address').textContent = `${profile.address.number} ${profile.address.street}`;
        document.getElementById('customer-rating').textContent = profile.rating?.toFixed(1) ?? 'N/A';
    } catch (err) {
        showError(err.message);
    }
}

async function initEditProfile() {
    try {
        const profile = await fetchCustomerProfile();
        const nameEl = document.getElementById('edit-name');
        const emailEl = document.getElementById('edit-email');
        const addressEl = document.getElementById('edit-address');
        nameEl.value = profile.username || profile.name || '';
        emailEl.value = profile.email || '';
        addressEl.value = profile.address || '';
        document.getElementById('edit-profile-form').addEventListener('submit', async e => {
            e.preventDefault();
            try {
                const data = {
                    name: nameEl.value,
                    email: emailEl.value,
                    address: addressEl.value
                };
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

async function initDishDetail() {
    try {
        const id = sessionStorage.getItem('currentDishId');
        const d = await fetchDishDetail(id);
        document.getElementById('dish-photo').src = d.photoUrl || '';
        document.getElementById('dish-name').textContent = d.name;
        document.getElementById('dish-type').textContent = `Type: ${d.type}`;
        document.getElementById('dish-price').textContent = `Price: €${d.price.toFixed(2)}`;
        document.getElementById('dish-expiry').textContent = d.expiry;
        document.getElementById('dish-nationality').textContent = `Cuisine: ${d.cuisine}`;
        document.getElementById('dish-ingredients').textContent = d.ingredients.join(', ');
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