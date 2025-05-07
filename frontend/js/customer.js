// Customer-side interactions: browsing, cart, checkout
import { fetchDishes, placeOrder } from './api.js';
import { showError, redirect } from './common.js';

// Initialize a page based on its name
export function initPage(page) {
    switch (page) {
        case 'browse-dishes': initBrowse(); break;
        case 'cart': initCart(); break;
        case 'checkout': initCheckout(); break;
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