export function showError(message) {
    alert(`Error: ${message}`);
}

export function showMessage(message) {
    alert(message);
}

export function clearContent() {
    document.getElementById('content').innerHTML = '';
}

export async function loadHTML(path) {
    const res = await fetch(path);
    if (!res.ok) throw new Error(`Cannot load ${path}`);
    return await res.text();
}
