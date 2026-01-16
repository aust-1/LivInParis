// Map & routing with Leaflet
import L from '../lib/leaflet/leaflet.js';
import { getRoute } from './api.js';

document.addEventListener('DOMContentLoaded', () => {
    const mapEl = document.getElementById('map');
    if (!mapEl) return;

    const map = L.map(mapEl).setView([48.8566, 2.3522], 12);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png').addTo(map);

    document.getElementById('route-form').addEventListener('submit', async e => {
        e.preventDefault();
        const from = e.target.from.value;
        const to = e.target.to.value;
        try {
            const route = await getRoute(from, to);
            const latlngs = route.stations.map(s => [s.latitudeRadians, s.longitudeRadians]);
            L.polyline(latlngs).addTo(map);

        } catch (err) {
            alert(err.message);
        }
    });
});