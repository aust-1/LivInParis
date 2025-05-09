import { login, register } from './api.js';
import { showError, redirect, setAuthToken } from './common.js';

document.addEventListener('submit', async e => {
    const form = e.target;
    if (form.id === 'login-form') {
        e.preventDefault();
        const name = form.name.value;
        const password = form.password.value;
        try {
            const data = await login(name, password);
            setAuthToken(data.token);
            redirect('#/customer/dashboard');
        } catch (err) {
            showError(err.message);
        }
    }
    if (form.id === 'register-form') {
        e.preventDefault();
        const payload = {
            username: form.name.value,
            password: form.password.value
        };
        try {
            const data = await register(payload);
            setAuthToken(data.token);
            redirect('#/customer/dashboard');
        } catch (err) {
            showError(err.message);
        }
    }
});

export function initPage(page) {
    document.querySelectorAll('.error-message').forEach(el => el.style.display = 'none');
}