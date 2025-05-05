import { loadClientsPage } from "./clients.js";
import { loadDishesPage } from "./dishes.js";

document.getElementById("nav-clients").onclick = () => loadClientsPage();
document.getElementById("nav-dishes").onclick = () => loadDishesPage();

loadClientsPage();

//TODO: faire le même pour les autres pages (dishes, orders, chefs…)