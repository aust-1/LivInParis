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
cd LivinParis
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

# Architecture

LivinParis
├─ docs
│  ├─ output_archive
│  │  ├─ graph_20250325_21-14-20.png
│  │  ├─ graph_20250325_22-09-45.png
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
│  │  │  ├─ Attributes
│  │  │  │  ├─ ConnectionControlAttributes.cs
│  │  │  │  └─ ConnectionInterceptor.cs
│  │  │  ├─ Interfaces
│  │  │  │  ├─ IAccountService.cs
│  │  │  │  ├─ IAddressService.cs
│  │  │  │  ├─ IChefService.cs
│  │  │  │  ├─ ICompanyService.cs
│  │  │  │  ├─ IContainsService.cs
│  │  │  │  ├─ ICustomerService.cs
│  │  │  │  ├─ IDishService.cs
│  │  │  │  ├─ IIndividualService.cs
│  │  │  │  ├─ IIngredientService.cs
│  │  │  │  ├─ IMenuProposalService.cs
│  │  │  │  ├─ IOrderLineService.cs
│  │  │  │  ├─ IReviewService.cs
│  │  │  │  └─ ITransactionService.cs
│  │  │  ├─ Repository.cs
│  │  │  └─ Services
│  │  │     ├─ AccountService.cs
│  │  │     ├─ AddressService.cs
│  │  │     ├─ ChefService.cs
│  │  │     ├─ CompanyService.cs
│  │  │     ├─ ContainsService.cs
│  │  │     ├─ CustomerService.cs
│  │  │     ├─ DishService.cs
│  │  │     ├─ IndividualService.cs
│  │  │     ├─ IngredientService.cs
│  │  │     ├─ MenuProposalService.cs
│  │  │     ├─ OrderLineService.cs
│  │  │     ├─ ReviewService.cs
│  │  │     └─ TransactionService.cs
│  │  ├─ LivinParis.csproj
│  │  ├─ Models
│  │  │  ├─ Enums
│  │  │  │  ├─ DishType.cs
│  │  │  │  ├─ LoyaltyRank.cs
│  │  │  │  ├─ OrderLineStatus.cs
│  │  │  │  ├─ ProductOrigin.cs
│  │  │  │  └─ ReviewType.cs
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
│  │  │     ├─ Customer.cs
│  │  │     ├─ Dish.cs
│  │  │     ├─ Individual.cs
│  │  │     ├─ Ingredient.cs
│  │  │     ├─ MenuProposal.cs
│  │  │     ├─ OrderLine.cs
│  │  │     ├─ Review.cs
│  │  │     └─ Transaction.cs
│  │  ├─ Program.cs
│  │  └─ Using.cs
│  ├─ output_graphs
│  │  ├─ graph_20250404_02-45-07.png
│  │  ├─ scc0_20250331_18-08-48.png
│  │  └─ ...
│  └─ resources
│     └─ MetroParis.xlsx
└─ tests
   └─ LivinParis.Tests
      ├─ EdgeTests.cs
      ├─ GraphOptimisation.cs
      ├─ GraphTests.cs
      ├─ LivinParis.Tests.csproj
      ├─ MSTestSettings.cs
      ├─ NodeTests.cs
      └─ Using.cs
