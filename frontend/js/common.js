// Utilities for UI feedback and auth token management
export function showError(msg) {
    const el = document.getElementById('error-message');
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