// Chef-side interactions: proposals, orders
import {
    fetchChefProposals, createProposal, deleteProposal,
    fetchIncomingOrders, acceptOrder, rejectOrder,
    fetchOrderDetail,
    fetchChefProfile, updateChefProfile
} from './api.js';

import { showError, redirect } from './common.js';

// Initialize page based on sub-route
export function initPage(page) {
    switch (page) {
        case 'dashboard':
            // Dashboard sub-nav links
            document.getElementById('nav-manage-menu')?.addEventListener('click', () => redirect('#/chef/manage-menu'));
            document.getElementById('nav-incoming-orders')?.addEventListener('click', () => redirect('#/chef/incoming-orders'));
            document.getElementById('nav-deliveries')?.addEventListener('click', () => redirect('#/chef/deliveries'));
            document.getElementById('nav-chef-profile')?.addEventListener('click', () => redirect('#/chef/profile'));
            break;
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
        const chefId = sessionStorage.getItem('accountId');
        if (!chefId) {
            showError('Missing chef id');
            return;
        }
        const proposals = await fetchChefProposals(chefId);
        tbody.innerHTML = '';
        proposals.forEach(p => {
            const tr = document.createElement('tr');
            tr.dataset.date = p.proposalDate;
            tr.innerHTML = `<td>${new Date(p.proposalDate).toLocaleDateString()}</td>
        <td>${p.dishes?.map(d => d.name).join(', ') || ''}</td>
        <td>
          <button class="btn-delete-proposal" data-date="${p.proposalDate}">Delete</button>
        </td>`;
            tbody.append(tr);
        });
        document.getElementById('btn-new-proposal')
            .addEventListener('click', () => redirect('#/chef/create-proposal'));
        tbody.addEventListener('click', async e => {
            const proposalDate = e.target.dataset.date;
            if (e.target.classList.contains('btn-delete-proposal')) {
                try {
                    await deleteProposal(chefId, proposalDate);
                    redirect('#/chef/manage-menu');
                } catch (err) { showError(err.message); }
            }
        });
    } catch (err) { showError(err.message); }
}


// Create new proposal
async function initCreateProposal() {
    const form = document.getElementById('create-proposal-form');
    form.addEventListener('submit', async e => {
        e.preventDefault();
        const chefId = sessionStorage.getItem('accountId');
        if (!chefId) {
            showError('Missing chef id');
            return;
        }
        const data = {
            chefId: parseInt(chefId, 10),
            proposalDate: form.date.value,
            dishId: parseInt(form.dish.value, 10)
        };
        try { await createProposal(chefId, data); redirect('#/chef/manage-menu'); }
        catch (err) { showError(err.message); }
    });
}


// Edit existing proposal
async function initEditProposal() {
    const form = document.getElementById('edit-proposal-form');
    form.addEventListener('submit', async e => {
        e.preventDefault();
        showError('Edit proposal not supported in API');
    });
}


// Incoming orders
async function initIncomingOrders() {
    try {
        const tbody = document.getElementById('incoming-orders-table').querySelector('tbody');
        const chefId = sessionStorage.getItem('accountId');
        if (!chefId) {
            showError('Missing chef id');
            return;
        }
        const orders = await fetchIncomingOrders(chefId);
        tbody.innerHTML = '';
        orders.forEach(o => {
            const tr = document.createElement('tr');
            tr.dataset.id = o.dishId;
            tr.innerHTML = `<td>${o.dishId}</td><td>-</td>
        <td>${new Date().toLocaleString()}</td>
        <td>${o.status}</td>
        <td><button class="btn-view-order" data-id="${o.dishId}">View</button></td>`;
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
        const chefId = sessionStorage.getItem('accountId');
        if (!chefId) {
            showError('Missing chef id');
            return;
        }
        const detail = await fetchOrderDetail(id);
        document.getElementById('chef-order-id').textContent = detail.id;
        document.getElementById('chef-order-customer').textContent = '-';
        document.getElementById('chef-order-date').textContent = new Date().toLocaleString();
        document.getElementById('chef-order-status').textContent = detail.lines?.[0]?.status || 'Pending';
        const tbody = document.getElementById('chef-order-items').querySelector('tbody');
        tbody.innerHTML = '';
        detail.lines?.forEach(it => {
            const row = document.createElement('tr');
            row.innerHTML = `<td>${it.dishName}</td><td>1</td><td>€${it.unitPrice.toFixed(2)}</td>`;
            tbody.append(row);
        });
        document.getElementById('btn-accept-order').addEventListener('click', async () => {
            try { await acceptOrder(chefId, id); redirect('#/chef/incoming-orders'); } catch (e) { showError(e.message); }
        });
        document.getElementById('btn-reject-order').addEventListener('click', async () => {
            try { await rejectOrder(chefId, id); redirect('#/chef/incoming-orders'); } catch (e) { showError(e.message); }
        });
    } catch (err) { showError(err.message); }
}


// My deliveries
async function initDeliveries() {
    try {
        showError('Delivery listing not available yet.');
    } catch (err) { showError(err.message); }
}

// Delivery detail
async function initDeliveryDetail() {
    try {
        showError('Delivery detail not available yet.');
    } catch (err) { showError(err.message); }
}


// Chef profile
async function initChefProfile() {
    try {
        const chefId = sessionStorage.getItem('accountId');
        if (!chefId) {
            showError('Missing chef id');
            return;
        }
        const prof = await fetchChefProfile(chefId);
        document.getElementById('chef-name').textContent = prof.username || '';
        document.getElementById('chef-email').textContent = '';
        document.getElementById('chef-address').textContent = `${prof.address?.number ?? ''} ${prof.address?.street ?? ''}`.trim();
        document.getElementById('chef-rating').textContent = prof.rating ?? '';
    } catch (err) { showError(err.message); }
}

// Edit chef profile
async function initEditChefProfile() {
    try {
        const form = document.getElementById('edit-chef-profile-form');
        const chefId = sessionStorage.getItem('accountId');
        if (!chefId) {
            showError('Missing chef id');
            return;
        }
        const prof = await fetchChefProfile(chefId);
        form.addressNumber.value = prof.address?.number || '';
        form.street.value = prof.address?.street || '';
        form.addEventListener('submit', async e => {
            e.preventDefault();
            const data = {
                isBanned: false,
                address: {
                    number: parseInt(form.addressNumber.value, 10),
                    street: form.street.value
                }
            };
            try { await updateChefProfile(chefId, data); redirect('#/chef/profile'); }
            catch (err) { showError(err.message); }
        });
    } catch (err) { showError(err.message); }
}


