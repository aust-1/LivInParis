import { getAuthToken, showError, clearAuthToken, redirect } from './common.js';

// Setup hash-based routing
window.addEventListener('load', loadPage);
window.addEventListener('hashchange', loadPage);
// Logout button
document.getElementById('logout-link')?.addEventListener('click', e => {
    e.preventDefault();
    clearAuthToken();
    sessionStorage.removeItem('accountId');
    redirect('#/auth/login');
});


async function loadPage() {
    const content = document.getElementById('content');
    let hash = window.location.hash || '#/auth/login';
    const [group, page] = hash.slice(2).split('/');
    const route = `${group}/${page || 'login'}`;
    // Authentication guard
    const publicRoutes = ['auth/login', 'auth/register'];
    if (!getAuthToken() && !publicRoutes.includes(route)) {
        window.location.hash = '#/auth/login';
        return;
    }
    const url = `pages/${group}/${page || 'login'}.html`;
    try {
        const res = await fetch(url);
        if (!res.ok) throw new Error();
        content.innerHTML = await res.text();
    } catch {
        const res404 = await fetch('pages/not-found.html');
        content.innerHTML = await res404.text();
    }
    // TODO: initialize page-specific scripts if needed
    try {
        if (group === 'auth') {
            import('./auth.js').then(m => {
                if (typeof m.initPage === 'function') m.initPage(page);
            });
        } else if (group === 'customer') {
            import('./customer.js').then(m => m.initPage(page));
        } else if (group === 'chef') {
            import('./chef.js').then(m => m.initPage(page));
        } else if (group === 'stats') {
            import('./stats.js').then(m => m.initPage(page));
        }
    } catch (err) {
        console.error('Init page failed', err);
    }
}