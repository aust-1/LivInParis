/* eslint-disable no-new */
// Statistics charts using Chart.js
import Chart from '../lib/chartjs/chart.umd.js';
import { fetchStatsOrdersByChef, fetchStatsRevenueByStreet, fetchStatsAverageOrderPrice, fetchStatsTopCuisines } from './api.js';

// Initialize stats pages
export function initPage(page) {
    switch (page) {
        case 'orders': initOrdersByChef(); break;
        case 'revenue': initRevenueByStreet(); break;
        case 'averages': initAverageOrderPrice(); break;
        case 'cuisine': initTopCuisines(); break;
        default: /* dashboard, nothing to init */ break;
    }
}

// Back to stats dashboard
document.addEventListener('click', e => {
    if (e.target.id === 'btn-back-stats-dashboard') {
        window.location.hash = '#/stats/dashboard';
    }
});

async function initOrdersByChef() {
    const ctx = document.getElementById('ordersByChefChart')?.getContext('2d');
    if (!ctx) return;
    const data = await fetchStatsOrdersByChef();
    // Assign chart instance to global to avoid unused instantiation errors
    window.ordersByChefChart = new Chart(ctx, { type: 'bar', data: { labels: data.map(d => d.chefName), datasets: [{ label: 'Orders', data: data.map(d => d.count) }] } });
}

async function initRevenueByStreet() {
    const ctx = document.getElementById('revenueByStreetChart')?.getContext('2d');
    if (!ctx) return;
    const data = await fetchStatsRevenueByStreet();
    window.revenueByStreetChart = new Chart(ctx, { type: 'bar', data: { labels: data.map(d => d.street), datasets: [{ label: 'Revenue (€)', data: data.map(d => d.total) }] } });
}

async function initAverageOrderPrice() {
    const ctx = document.getElementById('averageOrderPriceChart')?.getContext('2d');
    if (!ctx) return;
    const data = await fetchStatsAverageOrderPrice();
    window.averageOrderPriceChart = new Chart(ctx, { type: 'line', data: { labels: data.map(d => d.date), datasets: [{ label: 'Avg Price (€)', data: data.map(d => d.average) }] } });
}

async function initTopCuisines() {
    const ctx = document.getElementById('topCuisinesChart')?.getContext('2d');
    if (!ctx) return;
    const data = await fetchStatsTopCuisines();
    window.topCuisinesChart = new Chart(ctx, { type: 'pie', data: { labels: data.map(d => d.cuisine), datasets: [{ label: 'Top Cuisines', data: data.map(d => d.count) }] } });
}
