# LivInParis

Plateforme académique de livraison de repas dans Paris intra-muros. Le projet couvre un backend .NET 9 avec API REST, un frontend statique en JavaScript, une base MySQL et une couche métier orientée graphes (métro parisien, calculs de chemins, statistiques). L’objectif était de livrer une application complète, déployable via Docker, tout en explorant des sujets techniques avancés.

## Objectif du projet

- Simuler un service de livraison avec chefs, clients, menus et commandes.
- Exploiter le graphe du métro parisien pour des calculs de distance et d’optimisation.
- Produire des statistiques métier (revenus, préférences, livraisons).
- Proposer une interface web simple pour chaque rôle (chef, client, stats).

## Fonctionnalités principales

- Authentification JWT et rôles distincts.
- Gestion des profils chef/client, menus, plats, commandes et paiements.
- Pipeline de checkout et panier côté client.
- Statistiques métier exposées via l’API.
- Visualisation cartographique et dashboard (Leaflet + Chart.js).
- API REST documentée via Swagger en environnement dev.

## Stack technique

- Backend: .NET 9, ASP.NET Core Web API
- Données: MySQL, initialisation via `init.sql`
- ORM: Entity Framework Core
- Architecture: Domain, Infrastructure, Services, API
- Frontend: HTML/CSS/JS, fetch API, pages statiques servies par l’API
- Cartographie & charts: Leaflet, Chart.js
- Infra: Docker Compose, variables d’environnement, images dédiées

## Architecture (vue rapide)

- `src/LivInParis.Api`: contrôleurs, configuration, auth JWT, CORS, Swagger, hosting SPA.
- `src/LivInParis.Domain`: modèles métier et graphe du métro (algorithmes).
- `src/LivInParis.Infrastructure`: EF Core, repositories, accès DB.
- `src/LivInParis.Services`: logique métier, services applicatifs.
- `frontend`: pages statiques et scripts JS.

## Ce que j’ai appris (détails techniques)

- Concevoir une API REST en ASP.NET Core avec routing, DTO et Swagger.
- Mettre en place une authentification JWT (config, validation, tokens).
- Structurer un projet en couches (Domain/Infrastructure/Services/API) et séparer la logique métier de l’accès aux données.
- Implémenter un repository générique + repositories spécialisés pour les entités métier.
- Modéliser un graphe réel (métro de Paris) et implémenter des algorithmes de plus court chemin.
- Comparer des structures de données (matrice vs liste d’adjacence) et mesurer les performances.
- Produire des statistiques SQL et les exposer via services dédiés.
- Servir une SPA statique avec ASP.NET Core et gérer le proxy en dev.
- Orchestrer une stack complète avec Docker Compose (API + MySQL).
- Construire un frontend sans framework, en organisant le JS par domaine fonctionnel.

## Démarrage rapide

### Prérequis

- .NET 9 SDK
- Node.js (pour `serve`)
- Docker Desktop

### Lancer la base et l’API

```bash
docker compose up -d --build
dotnet run --project src/LivInParis.Api
```

### Lancer le frontend

```bash
cd frontend
npm install
npm start
```

### Accès

- Frontend: `http://localhost:62542/`
- API: `http://localhost:53754/` (Swagger en dev)

## Données et configuration

- Variables d’environnement: voir `.env.example`.
- Initialisation MySQL: `init.sql`.
- Le graphe du métro est alimenté par des fichiers Excel dans `resources`.

## Notes académiques

Le projet met l’accent sur l’algorithmique de graphes et l’architecture logicielle. Les rapports associés se trouvent dans `docs/` (optimisation de graphes, expérimentation IA).
