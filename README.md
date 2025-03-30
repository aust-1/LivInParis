# LivinParis

## Projet d'architecture

LivinParis
├─ docs
│  ├─ output_archive
│  │  ├─ graph_20250330_16-33-05.png
│  │  └─ ...
│  ├─ PSI.loo
│  └─ rapport_d_utilisation_de_l_ia.md
├─ LICENSE
├─ LivinParis.sln
├─ README.md
├─ src
│  ├─ database
│  │  ├─ .env
│  │  ├─ docker-compose.yml
│  │  └─ init.sql
│  └─ LivinParis
│     ├─ Controllers
│     ├─ data
│     │  ├─ metro
│     │  │  └─ MetroParis.xlsx
│     │  ├─ output
│     │  └─ Repositories
│     │     ├─ IRepository.cs
│     │     └─ Repository.cs
│     ├─ LivinParis.csproj
│     ├─ Models
│     │  ├─ Maps
│     │  │  ├─ Edge.cs
│     │  │  ├─ Graph.cs
│     │  │  ├─ Node.cs
│     │  │  ├─ Station.cs
│     │  │  └─ VisualizationParameters.cs
│     │  └─ Order
│     │     ├─ Account.cs
│     │     ├─ Adress.cs
│     │     ├─ Chef.cs
│     │     ├─ Company.cs
│     │     ├─ Customer.cs
│     │     ├─ Dish.cs
│     │     ├─ Individual.cs
│     │     ├─ Ingredient.cs
│     │     ├─ MenuProposal.cs
│     │     ├─ OrderLine.cs
│     │     ├─ Review.cs
│     │     └─ Transaction.cs
│     ├─ Program.cs
│     └─ Using.cs
└─ tests
   └─ LivinParis.Tests
      ├─ EdgeTests.cs
      ├─ GraphTests.cs
      ├─ LivinParis.Tests.csproj
      ├─ MSTestSettings.cs
      ├─ NodeTests.cs
      └─ Using.cs
