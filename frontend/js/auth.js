import { login, register } from './api.js';
import { showError, redirect, setAuthToken } from './common.js';

document.addEventListener('submit', async e => {
    const form = e.target;
    if (form.id === 'login-form') {
        e.preventDefault();
        const username = form.name.value;
        const password = form.password.value;
        try {
            const data = await login(username, password);
            setAuthToken(data.token);
            if (data.accountId) {
                sessionStorage.setItem('accountId', data.accountId);
            }
            redirect('#/customer/dashboard');
        } catch (err) {
            showError(err.message);
        }
    }
    if (form.id === 'register-form') {
        e.preventDefault();
        const payload = {
            username: form.name.value,
            password: form.password.value,
            email: form.email?.value,
            firstName: form.firstName?.value,
            lastName: form.lastName?.value,
            phoneNumber: form.phoneNumber?.value,
            addressNumber: form.addressNumber?.value ? parseInt(form.addressNumber.value, 10) : null,
            street: form.street?.value,
            isCompany: form.isCompany?.checked || false,
            companyName: form.companyName?.value,
            contactFirstName: form.contactFirstName?.value,
            contactLastName: form.contactLastName?.value
        };
        try {
            const data = await register(payload);
            setAuthToken(data.token);
            if (data.accountId) {
                sessionStorage.setItem('accountId', data.accountId);
            }
            redirect('#/customer/dashboard');
        } catch (err) {
            showError(err.message);
        }
    }
});

export function initPage(page) {
    document.querySelectorAll('.error-message').forEach(el => el.style.display = 'none');
}


export function initPage(page) {
    document.querySelectorAll('.error-message').forEach(el => el.style.display = 'none');
}