// Chef-side interactions: proposals, orders
import { fetchProposals } from './api.js';
import { showError } from './common.js';

document.addEventListener('DOMContentLoaded', () => {
    if (document.body.classList.contains('manage-menu')) loadProposals();
    if (document.body.classList.contains('incoming-orders')) loadOrders();
});

async function loadProposals() {
    try {
        const list = document.getElementById('proposals-list');
        const proposals = await fetchProposals();
        proposals.forEach(p => {
            const row = document.createElement('div');
            row.textContent = p.title + ' – ' + p.status;
            list.append(row);
        });
    } catch (err) {
        showError(err.message);
    }
}

function loadOrders() {
    // Similar logic to display incoming orders
}

