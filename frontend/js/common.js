// Utilities for UI feedback and auth token management
export function showError(msg) {
    // Display error message in the first visible .error-message element
    const el = document.querySelector('.error-message');
    if (el) { el.textContent = msg; el.style.display = 'block'; }
}

export function showSuccess(msg) {
    const el = document.getElementById('success-message');
    if (el) { el.textContent = msg; el.style.display = 'block'; }
}

export function redirect(path) {
    if (path.startsWith('#')) {
        window.location.hash = path;
    } else {
        window.location.href = path;
    }
}

export function setAuthToken(token) {
    sessionStorage.setItem('authToken', token);
}

export function getAuthToken() {
    return sessionStorage.getItem('authToken');
}

export function clearAuthToken() {
    sessionStorage.removeItem('authToken');
}

// Cart management
export function getCart() {
    const c = sessionStorage.getItem('cart');
    return c ? JSON.parse(c) : [];
}
export function saveCart(cart) {
    sessionStorage.setItem('cart', JSON.stringify(cart));
}