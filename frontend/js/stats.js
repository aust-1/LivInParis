// Statistics charts using Chart.js
import Chart from '../lib/chartjs/chart.umd.js';
import { /* fetchStats... */ } from './api.js';

document.addEventListener('DOMContentLoaded', async () => {
    const ctx1 = document.getElementById('ordersByChefChart')?.getContext('2d');
    if (ctx1) {
        const data = await fetchStatsOrdersByChef();
        new Chart(ctx1, {
            type: 'bar', data: {
                labels: data.map(d => d.chefName),
                datasets: [{ label: 'Orders', data: data.map(d => d.count) }]
            }
        });
    }
    // Repeat for other stat charts...
});
