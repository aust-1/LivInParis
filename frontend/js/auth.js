// Handles login and registration forms
import { login, register } from './api.js';
import { showError, redirect, setAuthToken } from './common.js';

// Handle login and register forms dynamically loaded
document.addEventListener('submit', async e => {
    const form = e.target;
    if (form.id === 'login-form') {
        e.preventDefault();
        const name = form.name.value;
        const password = form.password.value;
        try {
            const data = await login(name, password);
            setAuthToken(data.token);
            redirect(data.role === 'chef' ? '#/chef/dashboard' : '#/customer/dashboard');
        } catch (err) {
            showError(err.message);
        }
    }
    if (form.id === 'register-form') {
        e.preventDefault();
        const payload = {
            name: form.name.value,
            role: form.role.value,
            password: form.password.value
        };
        try {
            const data = await register(payload);
            setAuthToken(data.token);
            redirect(data.role === 'chef' ? '#/chef/dashboard' : '#/customer/dashboard');
        } catch (err) {
            showError(err.message);
        }
    }
});