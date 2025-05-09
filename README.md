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


# C) Navigateur
open http://localhost:62542/
```

#### 1. Cloner le dépôt

```bash
git clone https://github.com/Captainbleu/LivInParis.git
cd .\LivInParis\
```

#### 2. Démarrer les conteneurs

```bash
docker compose up -d --build
dotnet run --project src/LivinParis.Api
cd frontend
npm install
npm start
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
^C
docker compose down
```

Ajoutez `--volumes` si vous souhaitez supprimer les volumes (⚠️ perte de données) :

```bash
docker compose down --volumes
```

## Explications supplémentaires

Nous n'avions pas conscience qu'il fallait faire la logique métier pour ce rendu, nous nous sommes donc concentré sur tous les objets métiers et la base de données. Nous vous invitons donc à lire le code notamment dans le dossier `src/LivInParis/Models` pour les graphes, stations, la détection automatique de la station la plus proche et les objets métiers, et dans le dossier `src/LivInParis/data` pour la base de donnée. Nous avons implémenté énormément de requête SQL pour faire des statistiques. Nous avons développé un attribute `ConnectionInterceptor` qui nous permet de faire des requêtes SQL avant et après chaque appel de méthode dans le repository. Cela nous permet de mieux encapsuler et centraliser la gestion de la connexion à la base de données.

Bonne lecture !
