# LivInParis

Bienvenue dans le projet **LivInParis**, une plateforme de livraison de repas dans Paris intra muros.

## 👥 Nous

Captainbleu (Austin) : Eliott Roussille

proxy-1 : François Teynier

## 🚀 Démarrage rapide - Docker Desktop

Ce guide vous aide à démarrer rapidement l’environnement de développement avec Docker.

---

### 🐳 Prérequis

Assurez-vous d’avoir installé :

- [Docker](https://www.docker.com/products/docker-desktop/)
- [Docker Compose](https://docs.docker.com/compose/install/) (souvent inclus avec Docker Desktop)

Ou avec :

```bash
winget install -e --id Docker.DockerDesktop
```

---

### 🚀 Lancer le projet

```bash
# 1. Arrêt complet
# → Dans le terminal du front   : Ctrl + C
# → À la racine du projet       : docker compose down [--volumes]
# → (Optionnel si dotnet run)   : Ctrl + C ou Stop-Process dotnet

# 2. Démarrage
# A) Base + API via Docker
docker compose up -d --build
dotnet run --project src/LivinParis.Api

# B) Front dans un autre terminal
cd frontend
npm start

# C) Navigateur
open http://localhost:62542/
```

#### 1. Cloner le dépôt

```bash
git clone https://github.com/Captainbleu/LivInParis.git
cd .\LivInParis\src\database\
```

#### 2. Démarrer les conteneurs

```bash
docker compose up -d
```

Cela va :

- Démarrer une instance MySQL préconfigurée
- Créer les volumes nécessaires pour la persistance
- Exposer le port de la base de données (`3306` par défaut)

#### 3. (Optionnel) Vérifier l’état

```bash
docker compose ps
```

---

### 🛠️ Détails techniques

| Service     | Port | Description                  |
|-------------|------|------------------------------|
| `mysql`     | 3306 | Base de données MySQL        |

Les identifiants par défaut (définis dans `docker-compose.yml`) sont :

```env
DB_HOST=localhost
DB_ROOT_PASSWORD=451520
DB_USER=livinuser
DB_PASSWORD=postgresbatmysql
DB_NAME=livinparisroussilleteynier
DB_PORT=3306
```

---

### 🧹 Arrêter et nettoyer

```bash
docker compose down
```

Ajoutez `--volumes` si vous souhaitez supprimer les volumes (⚠️ perte de données) :

```bash
docker compose down --volumes
```

## Explications supplémentaires

Nous n'avions pas conscience qu'il fallait faire la logique métier pour ce rendu, nous nous sommes donc concentré sur tous les objets métiers et la base de données. Nous vous invitons donc à lire le code notamment dans le dossier `src/LivInParis/Models` pour les graphes, stations, la détection automatique de la station la plus proche et les objets métiers, et dans le dossier `src/LivInParis/data` pour la base de donnée. Nous avons implémenté énormément de requête SQL pour faire des statistiques. Nous avons développé un attribute `ConnectionInterceptor` qui nous permet de faire des requêtes SQL avant et après chaque appel de méthode dans le repository. Cela nous permet de mieux encapsuler et centraliser la gestion de la connexion à la base de données.

Bonne lecture !

## Architecture

LivInParis
├─ .env
├─ docker-compose.yml
├─ docs
│  ├─ LivInParisFrontEndArchi.dot
│  ├─ LivInParisFrontEndArchi.svg
│  ├─ output_archive
│  │  ├─ graph_20250325_21-14-20.png
│  │  ├─ graph_20250325_22-09-45.png
│  │  ├─ welshpowell_20250428_18-29-40.png
│  │  └─ ...
│  ├─ PSI.loo
│  ├─ rapport_d_optimisation_de_graph.md
│  └─ rapport_d_utilisation_de_l_ia.md
├─ frontend
│  ├─ css
│  │  └─ style.css
│  ├─ index.html
│  ├─ js
│  │  ├─ api.js
│  │  ├─ app.js
│  │  ├─ auth.js
│  │  ├─ chef.js
│  │  ├─ common.js
│  │  ├─ customer.js
│  │  ├─ map.js
│  │  └─ stats.js
│  ├─ lib
│  │  ├─ chartjs
│  │  │  ├─ chart.umd.js
│  │  │  └─ chart.umd.js.map
│  │  └─ leaflet
│  │     ├─ images
│  │     │  ├─ layers-2x.png
│  │     │  ├─ layers.png
│  │     │  ├─ marker-icon-2x.png
│  │     │  ├─ marker-icon.png
│  │     │  └─ marker-shadow.png
│  │     ├─ leaflet.css
│  │     ├─ leaflet.js
│  │     └─ leaflet.js.map
│  └─ pages
│     ├─ auth
│     │  ├─ login.html
│     │  └─ register.html
│     ├─ chef
│     │  ├─ create-proposal.html
│     │  ├─ dashboard.html
│     │  ├─ delivery-detail.html
│     │  ├─ edit-profile.html
│     │  ├─ edit-proposal.html
│     │  ├─ incoming-orders.html
│     │  ├─ manage-menu.html
│     │  ├─ my-deliveries.html
│     │  ├─ order-detail.html
│     │  └─ profile.html
│     ├─ customer
│     │  ├─ browse-dishes.html
│     │  ├─ cart.html
│     │  ├─ checkout.html
│     │  ├─ dashboard.html
│     │  ├─ dish-detail.html
│     │  ├─ edit-profile.html
│     │  ├─ my-orders.html
│     │  ├─ order-confirmation.html
│     │  ├─ order-detail.html
│     │  └─ profile.html
│     ├─ not-found.html
│     └─ stats
│        └─ dashboard.html
├─ init.sql
├─ LICENSE
├─ LivInParis.sln
├─ README.md
├─ resources
│  ├─ dish_pictures
│  │  ├─ null.jpg
│  │  ├─ plat_1.jpg
│  │  └─ ...
│  ├─ MetroParis.xlsx
│  └─ Peuplement.xlsx
└─ src
   ├─ LivInParis.Api
   │  ├─ appsettings.Development.json
   │  ├─ appsettings.json
   │  ├─ Controllers
   │  │  └─ AddressesController.cs
   │  ├─ Dockerfile
   │  ├─ LivInParis.Api.csproj
   │  ├─ Program.cs
   │  └─ Properties
   │     └─ launchSettings.json
   ├─ LivInParis.Domain
   │  ├─ LivInParis.Domain.csproj
   │  └─ Models
   │     ├─ Maps
   │     │  ├─ Edge.cs
   │     │  ├─ Graph.cs
   │     │  ├─ Helpers
   │     │  │  ├─ CycleDetector.cs
   │     │  │  ├─ GraphAlgorithms.cs
   │     │  │  ├─ PathfindingResult.cs
   │     │  │  ├─ Visualization.cs
   │     │  │  └─ VisualizationParameters.cs
   │     │  ├─ Metro.cs
   │     │  ├─ Node.cs
   │     │  └─ Station.cs
   │     └─ Order
   │        ├─ Account.cs
   │        ├─ Address.cs
   │        ├─ Chef.cs
   │        ├─ Company.cs
   │        ├─ Contains.cs
   │        ├─ Customer.cs
   │        ├─ Dish.cs
   │        ├─ Enums
   │        │  ├─ DishType.cs
   │        │  ├─ OrderLineStatus.cs
   │        │  ├─ ProductsOrigin.cs
   │        │  └─ ReviewerType.cs
   │        ├─ Individual.cs
   │        ├─ Ingredient.cs
   │        ├─ MenuProposal.cs
   │        ├─ OrderLine.cs
   │        ├─ OrderTransaction.cs
   │        └─ Review.cs
   ├─ LivInParis.Infrastructure
   │  ├─ Data
   │  │  └─ LivInParisContext.cs
   │  ├─ Interfaces
   │  │  ├─ IAccountRepository.cs
   │  │  ├─ IAddressRepository.cs
   │  │  ├─ IChefRepository.cs
   │  │  ├─ ICompanyRepository.cs
   │  │  ├─ IContainsRepository.cs
   │  │  ├─ ICustomerRepository.cs
   │  │  ├─ IDishRepository.cs
   │  │  ├─ IIndividualRepository.cs
   │  │  ├─ IIngredientRepository.cs
   │  │  ├─ IMenuProposalRepository.cs
   │  │  ├─ IOrderLineRepository.cs
   │  │  ├─ IOrderTransactionRepository.cs
   │  │  ├─ IRepository.cs
   │  │  └─ IReviewRepository.cs
   │  ├─ LivInParis.Infrastructure.csproj
   │  └─ Repositories
   │     ├─ AccountRepository.cs
   │     ├─ AddressRepository.cs
   │     ├─ ChefRepository.cs
   │     ├─ CompanyRepository.cs
   │     ├─ ContainsRepository.cs
   │     ├─ CustomerRepository.cs
   │     ├─ DishRepository.cs
   │     ├─ IndividualRepository.cs
   │     ├─ IngredientRepository.cs
   │     ├─ MenuProposalRepository.cs
   │     ├─ OrderLineRepository.cs
   │     ├─ OrderTransactionRepository.cs
   │     ├─ Repository.cs
   │     └─ ReviewRepository.cs
   ├─ LivInParis.Services
   │  ├─ ExportService.cs
   │  ├─ Interfaces
   │  │  ├─ IAccountService.cs
   │  │  ├─ IAddressService.cs
   │  │  ├─ IChefService.cs
   │  │  ├─ IContainsService.cs
   │  │  ├─ ICustomerService.cs
   │  │  ├─ IDishService.cs
   │  │  ├─ IGraphService.cs
   │  │  ├─ IMenuProposalService.cs
   │  │  ├─ IOrderLineService.cs
   │  │  ├─ IReviewService.cs
   │  │  └─ IStatisticsService.cs
   │  ├─ LivInParis.Services.csproj
   │  └─ Services
   │     ├─ AccountService.cs
   │     ├─ AddressService.cs
   │     ├─ ChefService.cs
   │     ├─ ContainsService.cs
   │     ├─ CustomerService.cs
   │     ├─ DishService.cs
   │     ├─ GraphService.cs
   │     ├─ MenuProposalService.cs
   │     ├─ OrderLineService.cs
   │     ├─ ReviewService.cs
   │     └─ StatisticsService.cs
   └─ LivInParis.Tests
      ├─ LivInParis.Tests.csproj
      ├─ MSTestSettings.cs
      └─ ...
