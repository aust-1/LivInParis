# LivinParis

Bienvenue dans le projet **LivinParis**, une plateforme de livraison de repas dans Paris intra muros.

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

#### 1. Cloner le dépôt

```bash
git clone https://github.com/Captainbleu/LivinParis.git
cd .\LivinParis\src\database\
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

Nous n'avions pas conscience qu'il fallait faire la logique métier pour ce rendu, nous nous sommes donc concentré sur tous les objets métiers et la base de données. Nous vous invitons donc à lire le code notamment dans le dossier `src/LivinParis/Models` pour les graphes, stations, la détection automatique de la station la plus proche et les objets métiers, et dans le dossier `src/LivinParis/data` pour la base de donnée. Nous avons implémenté énormément de requête SQL pour faire des statistiques. Nous avons développé un attribute `ConnectionInterceptor` qui nous permet de faire des requêtes SQL avant et après chaque appel de méthode dans le repository. Cela nous permet de mieux encapsuler et centraliser la gestion de la connexion à la base de données.

Bonne lecture !

## Architecture

LivinParis
├─ docs
│  ├─ output_archive
│  │  ├─ graph_20250325_21-14-20.png
│  │  ├─ graph_20250325_22-09-45.png
│  │  ├─ welshpowell_20250428_18-29-40.png
│  │  └─ ...
│  ├─ PSI.loo
│  ├─ rapport_d_optimisation_de_graph.md
│  └─ rapport_d_utilisation_de_l_ia.md
├─ LICENSE
├─ LivinParis.sln
├─ README.md
├─ src
│  ├─ database
│  │  ├─ .env
│  │  ├─ docker-compose.yml
│  │  └─ init.sql
│  ├─ LivinParis
│  │  ├─ data
│  │  │  ├─ Interfaces
│  │  │  │  ├─ IAccountRepository.cs
│  │  │  │  ├─ IAddressRepository.cs
│  │  │  │  ├─ IChefRepository.cs
│  │  │  │  ├─ ICompanyRepository.cs
│  │  │  │  ├─ IContainsRepository.cs
│  │  │  │  ├─ ICustomerRepository.cs
│  │  │  │  ├─ IDishRepository.cs
│  │  │  │  ├─ IIndividualRepository.cs
│  │  │  │  ├─ IIngredientRepository.cs
│  │  │  │  ├─ IMenuProposalRepository.cs
│  │  │  │  ├─ IOrderLineRepository.cs
│  │  │  │  ├─ IOrderTransactionRepository.cs
│  │  │  │  ├─ IRepository.cs
│  │  │  │  └─ IReviewRepository.cs
│  │  │  ├─ LivInParisContext.cs
│  │  │  ├─ Repositories
│  │  │  │  ├─ AccountRepository.cs
│  │  │  │  ├─ AddressRepository.cs
│  │  │  │  ├─ ChefRepository.cs
│  │  │  │  ├─ CompanyRepository.cs
│  │  │  │  ├─ ContainsRepository.cs
│  │  │  │  ├─ CustomerRepository.cs
│  │  │  │  ├─ DishRepository.cs
│  │  │  │  ├─ IndividualRepository.cs
│  │  │  │  ├─ IngredientRepository.cs
│  │  │  │  ├─ MenuProposalRepository.cs
│  │  │  │  ├─ OrderLineRepository.cs
│  │  │  │  ├─ OrderTransactionRepository.cs
│  │  │  │  ├─ Repository.cs
│  │  │  │  └─ ReviewRepository.cs
│  │  │  └─ Repository.cs
│  │  ├─ DataBaseSeeder.cs
│  │  ├─ LivinParis.csproj
│  │  ├─ Models
│  │  │  ├─ Maps
│  │  │  │  ├─ Edge.cs
│  │  │  │  ├─ Graph.cs
│  │  │  │  ├─ Helpers
│  │  │  │  │  ├─ CycleDetector.cs
│  │  │  │  │  ├─ GraphAlgorithms.cs
│  │  │  │  │  ├─ PathfindingResult.cs
│  │  │  │  │  ├─ Visualization.cs
│  │  │  │  │  └─ VisualizationParameters.cs
│  │  │  │  ├─ Metro.cs
│  │  │  │  ├─ Node.cs
│  │  │  │  └─ Station.cs
│  │  │  └─ Order
│  │  │     ├─ Account.cs
│  │  │     ├─ Address.cs
│  │  │     ├─ Chef.cs
│  │  │     ├─ Company.cs
│  │  │     ├─ Contains.cs
│  │  │     ├─ Customer.cs
│  │  │     ├─ Dish.cs
│  │  │     ├─ Enums
│  │  │     │  ├─ DishType.cs
│  │  │     │  ├─ LoyaltyRank.cs
│  │  │     │  ├─ OrderLineStatus.cs
│  │  │     │  ├─ ProductsOrigin.cs
│  │  │     │  └─ ReviewerType.cs
│  │  │     ├─ Individual.cs
│  │  │     ├─ Ingredient.cs
│  │  │     ├─ MenuProposal.cs
│  │  │     ├─ OrderLine.cs
│  │  │     ├─ OrderTransaction.cs
│  │  │     └─ Review.cs
│  │  ├─ Program.cs
│  │  └─ Using.cs
│  ├─ output_graphs
│  │  ├─ ...
│  │  └─ graph_20250404_02-45-07.png
│  └─ resources
│     ├─ dish_pictures
│     │  ├─ null.jpg
│     │  ├─ plat_1.jpg
│     │  ├─ plat_2.jpg
│     │  └─ plat_3.jpg
│     ├─ MetroParis.xlsx
│     └─ Peuplement.xlsx
└─ tests
   └─ LivinParis.Tests
      ├─ AccountServiceTests.cs
      ├─ EdgeTests.cs
      ├─ GraphOptimisation.cs
      ├─ GraphTests.cs
      ├─ LivinParis.Tests.csproj
      ├─ Models
      │  └─ Order
      │     ├─ AccountTests.cs
      │     ├─ AddressTests.cs
      │     ├─ CustomerTests.cs
      │     ├─ DishTests.cs
      │     ├─ IngredientTests.cs
      │     └─ Utils.cs
      ├─ MSTestSettings.cs
      ├─ NodeTests.cs
      └─ Using.cs
