# LivinParis

## Projet d'architecture

LivinParis
├─ docs
│  ├─ output_archive
│  │  ├─ graph_20250330_16-33-05.png
│  │  └─ ...
│  ├─ PSI.loo
│  ├─ rapport_d_optimisation_de_graph.md
│  ├─ rapport_d_utilisation_de_l_ia.md
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
│     │  │  ├─ graph_20250402_15-43-13.png
│     │  │  └─ ...
│     │  └─ Repositories
│     │     ├─ Implementations
│     │     │  ├─ AccountRepository.cs
│     │     │  ├─ AdressRepository.cs
│     │     │  ├─ ChefRepository.cs
│     │     │  ├─ CompanyRepository.cs
│     │     │  ├─ ContainsRepository.cs
│     │     │  ├─ CustomerRepository.cs
│     │     │  ├─ DishRepository.cs
│     │     │  ├─ IndividualRepository.cs
│     │     │  ├─ IngredientRepository.cs
│     │     │  ├─ MenuProposalRepository.cs
│     │     │  ├─ OrderLineRepository.cs
│     │     │  ├─ ReviewRepository.cs
│     │     │  └─ TransactionRepository.cs
│     │     └─ Interfaces
│     │        ├─ IAccountRepository.cs
│     │        ├─ IAdressRepository.cs
│     │        ├─ IChefRepository.cs
│     │        ├─ ICompanyRepository.cs
│     │        ├─ IContainsRepository.cs
│     │        ├─ ICustomerRepository.cs
│     │        ├─ IDishRepository.cs
│     │        ├─ IIndividualRepository.cs
│     │        ├─ IIngredientRepository.cs
│     │        ├─ IMenuProposalRepository.cs
│     │        ├─ IOrderLineRepository.cs
│     │        ├─ IReviewRepository.cs
│     │        └─ ITransactionRepository.cs
│     ├─ LivinParis.csproj
│     ├─ Models
│     │  ├─ Maps
│     │  │  ├─ Edge.cs
│     │  │  ├─ Graph.cs
│     │  │  ├─ Helpers
│     │  │  │  ├─ CycleDetector.cs
│     │  │  │  ├─ GraphAlgorithms.cs
│     │  │  │  ├─ PathfindingResult.cs
│     │  │  │  ├─ Visualization.cs
│     │  │  │  └─ VisualizationParameters.cs
│     │  │  ├─ Metro.cs
│     │  │  ├─ Node.cs
│     │  │  └─ Station.cs
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
      ├─ GraphOptimisation.cs
      ├─ GraphTests.cs
      ├─ LivinParis.Tests.csproj
      ├─ MSTestSettings.cs
      ├─ NodeTests.cs
      └─ Using.cs
