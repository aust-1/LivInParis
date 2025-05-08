// Chef-side interactions: proposals, orders
import {
    fetchChefProposals, createProposal, updateProposal, deleteProposal,
    fetchIncomingOrders, acceptOrder, rejectOrder,
    fetchDeliveries, fetchDeliveryDetail,
    fetchChefProfile, updateChefProfile
} from './api.js';
import { showError, redirect } from './common.js';

// Initialize page based on sub-route
export function initPage(page) {
    switch (page) {
        case 'dashboard': /* nothing to init */ break;
        case 'manage-menu': initManageMenu(); break;
        case 'create-proposal': initCreateProposal(); break;
        case 'edit-proposal': initEditProposal(); break;
        case 'incoming-orders': initIncomingOrders(); break;
        case 'order-detail': initChefOrderDetail(); break;
        case 'deliveries': initDeliveries(); break;
        case 'delivery-detail': initDeliveryDetail(); break;
        case 'profile': initChefProfile(); break;
        case 'edit-profile': initEditChefProfile(); break;
    }
    // Global navigation buttons
    document.addEventListener('click', e => {
        const id = e.target.id;
        if (id === 'btn-back-chef-dashboard') redirect('#/chef/dashboard');
        if (id === 'btn-cancel-create') redirect('#/chef/manage-menu');
        if (id === 'btn-cancel-edit-proposal') redirect('#/chef/manage-menu');
        if (id === 'btn-cancel-deliveries') redirect('#/chef/dashboard');
        if (id === 'btn-cancel-edit-chef') redirect('#/chef/profile');
    });
}

// Manage menu proposals
async function initManageMenu() {
    try {
        const tbody = document.getElementById('proposals-table').querySelector('tbody');
        const proposals = await fetchChefProposals();
        proposals.forEach(p => {
            const tr = document.createElement('tr');
            tr.dataset.id = p.id;
            tr.innerHTML = `<td>${new Date(p.date).toLocaleDateString()}</td>
        <td>${p.dishName}</td>
        <td>
          <button class="btn-edit-proposal" data-id="${p.id}">Edit</button>
          <button class="btn-delete-proposal" data-id="${p.id}">Delete</button>
        </td>`;
            tbody.append(tr);
        });
        document.getElementById('btn-new-proposal')
            .addEventListener('click', () => redirect('#/chef/create-proposal'));
        tbody.addEventListener('click', async e => {
            const id = e.target.dataset.id;
            if (e.target.classList.contains('btn-edit-proposal')) {
                sessionStorage.setItem('currentProposalId', id);
                redirect('#/chef/edit-proposal');
            }
            if (e.target.classList.contains('btn-delete-proposal')) {
                try { await deleteProposal(id); redirect('#/chef/manage-menu'); }
                catch (err) { showError(err.message); }
            }
        });
    } catch (err) { showError(err.message); }
}

// Create new proposal
async function initCreateProposal() {
    const form = document.getElementById('create-proposal-form');
    form.addEventListener('submit', async e => {
        e.preventDefault();
        const data = { date: form.date.value, dish: form.dish.value };
        try { await createProposal(data); redirect('#/chef/manage-menu'); }
        catch (err) { showError(err.message); }
    });
}

// Edit existing proposal
async function initEditProposal() {
    const form = document.getElementById('edit-proposal-form');
    const id = sessionStorage.getItem('currentProposalId');
    // Optionally fetch details to prefill
    form.addEventListener('submit', async e => {
        e.preventDefault();
        const data = { date: form.date.value, dish: form.dish.value };
        try { await updateProposal(id, data); redirect('#/chef/manage-menu'); }
        catch (err) { showError(err.message); }
    });
}

// Incoming orders
async function initIncomingOrders() {
    try {
        const tbody = document.getElementById('incoming-orders-table').querySelector('tbody');
        const orders = await fetchIncomingOrders();
        orders.forEach(o => {
            const tr = document.createElement('tr'); tr.dataset.id = o.id;
            tr.innerHTML = `<td>${o.id}</td><td>${o.customerName}</td>
        <td>${new Date(o.date).toLocaleString()}</td>
        <td>${o.status}</td>
        <td><button class="btn-view-order" data-id="${o.id}">View</button></td>`;
            tbody.append(tr);
        });
        tbody.addEventListener('click', e => {
            if (e.target.classList.contains('btn-view-order')) {
                sessionStorage.setItem('currentOrderId', e.target.dataset.id);
                redirect('#/chef/order-detail');
            }
        });
    } catch (err) { showError(err.message); }
}

// Chef order detail
async function initChefOrderDetail() {
    try {
        const id = sessionStorage.getItem('currentOrderId');
        const detail = await fetchOrderDetail(id); // reuse client API or create new
        document.getElementById('chef-order-id').textContent = detail.id;
        document.getElementById('chef-order-customer').textContent = detail.customerName;
        document.getElementById('chef-order-date').textContent = new Date(detail.date).toLocaleString();
        document.getElementById('chef-order-status').textContent = detail.status;
        const tbody = document.getElementById('chef-order-items').querySelector('tbody');
        detail.items.forEach(it => {
            const row = document.createElement('tr');
            row.innerHTML = `<td>${it.dishName}</td><td>${it.quantity}</td><td>€${it.subtotal.toFixed(2)}</td>`;
            tbody.append(row);
        });
        document.getElementById('btn-accept-order').addEventListener('click', async () => {
            try { await acceptOrder(id); redirect('#/chef/incoming-orders'); } catch (e) { showError(e.message); }
        });
        document.getElementById('btn-reject-order').addEventListener('click', async () => {
            try { await rejectOrder(id); redirect('#/chef/incoming-orders'); } catch (e) { showError(e.message); }
        });
    } catch (err) { showError(err.message); }
}

// My deliveries
async function initDeliveries() {
    try {
        const tbody = document.getElementById('deliveries-table').querySelector('tbody');
        const list = await fetchDeliveries();
        list.forEach(d => {
            const tr = document.createElement('tr'); tr.dataset.id = d.id;
            tr.innerHTML = `<td>${d.id}</td><td>${d.orderId}</td>
        <td>${new Date(d.date).toLocaleString()}</td>
        <td>${d.status}</td>
        <td><button class="btn-view-delivery" data-id="${d.id}">View</button></td>`;
            tbody.append(tr);
        });
        tbody.addEventListener('click', e => {
            if (e.target.classList.contains('btn-view-delivery')) {
                sessionStorage.setItem('currentDeliveryId', e.target.dataset.id);
                redirect('#/chef/delivery-detail');
            }
        });
    } catch (err) { showError(err.message); }
}

// Delivery detail
async function initDeliveryDetail() {
    try {
        const id = sessionStorage.getItem('currentDeliveryId');
        const d = await fetchDeliveryDetail(id);
        document.getElementById('delivery-id').textContent = d.id;
        document.getElementById('delivery-order-id').textContent = d.orderId;
        document.getElementById('delivery-address').textContent = d.address;
        document.getElementById('delivery-status').textContent = d.status;
    } catch (err) { showError(err.message); }
}

// Chef profile
async function initChefProfile() {
    try {
        const prof = await fetchChefProfile();
        document.getElementById('chef-name').textContent = prof.name;
        document.getElementById('chef-email').textContent = prof.email;
        document.getElementById('chef-address').textContent = prof.address;
        document.getElementById('chef-rating').textContent = prof.rating;
    } catch (err) { showError(err.message); }
}

// Edit chef profile
async function initEditChefProfile() {
    try {
        const form = document.getElementById('edit-chef-profile-form');
        const prof = await fetchChefProfile();
        form.name.value = prof.name;
        form.address.value = prof.address;
        form.email.value = prof.email;
        form.addEventListener('submit', async e => {
            e.preventDefault();
            const data = { name: form.name.value, address: form.address.value, email: form.email.value };
            try { await updateChefProfile(data); redirect('#/chef/profile'); }
            catch (err) { showError(err.message); }
        });
    } catch (err) { showError(err.message); }
}

