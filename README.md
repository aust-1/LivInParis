# LivinParis

## Projet d'architecture

LivinParis
├─ docs
│  ├─ output_archive
│  │  ├─ graph_20250330_16-09-48.png
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
│  └─ LivinParis
│     ├─ Controllers
│     ├─ data
│     │  ├─ Attributes
│     │  │  ├─ ConnectionControlAttributes.cs
│     │  │  └─ ConnectionInterceptor.cs
│     │  ├─ Interfaces
│     │  │  ├─ IAccountService.cs
│     │  │  ├─ IAdressService.cs
│     │  │  ├─ IChefService.cs
│     │  │  ├─ ICompanyService.cs
│     │  │  ├─ IContainsService.cs
│     │  │  ├─ ICustomerService.cs
│     │  │  ├─ IDishService.cs
│     │  │  ├─ IIndividualService.cs
│     │  │  ├─ IIngredientService.cs
│     │  │  ├─ IMenuProposalService.cs
│     │  │  ├─ IOrderLineService.cs
│     │  │  ├─ IReviewService.cs
│     │  │  ├─ IService.cs
│     │  │  └─ ITransactionService.cs
│     │  ├─ metro
│     │  │  └─ MetroParis.xlsx
│     │  ├─ output
│     │  │  ├─ graph_20250401_16-14-59.png
│     │  │  ├─ graph_20250402_15-12-19.png
│     │  │  ├─ graph_20250402_15-43-13.png
│     │  │  ├─ graph_20250402_18-05-43.png
│     │  │  ├─ scc0_20250331_18-08-48.png
│     │  │  └─ ...
│     │  ├─ Repository.cs
│     │  └─ Services
│     │     ├─ AccountService.cs
│     │     ├─ AdressService.cs
│     │     ├─ ChefService.cs
│     │     ├─ CompanyService.cs
│     │     ├─ ContainsService.cs
│     │     ├─ CustomerService.cs
│     │     ├─ DishService.cs
│     │     ├─ IndividualService.cs
│     │     ├─ IngredientService.cs
│     │     ├─ MenuProposalService.cs
│     │     ├─ OrderLineService.cs
│     │     ├─ ReviewService.cs
│     │     └─ TransactionService.cs
│     ├─ LivinParis.csproj
│     ├─ Models
│     │  ├─ Enums
│     │  │  ├─ DishType.cs
│     │  │  ├─ LoyaltyRank.cs
│     │  │  ├─ OrderLineStatus.cs
│     │  │  ├─ ProductOrigin.cs
│     │  │  └─ ReviewType.cs
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
│     │     ├─ Address.cs
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
