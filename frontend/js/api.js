const API = "http://localhost:5105/api";

export async function getClients() {
    const res = await fetch(`${API}/clients`);
    return res.json();
}

export async function createClient(client) {
    await fetch(`${API}/clients`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(client)
    });
}

//TODO: idem pour chefs, dishes, orders…
