# LivinParis

## Projet d'architecture

LivinParis
├─ docs
│  └─ PSI.loo
├─ LivinParis.sln
├─ README.md
├─ src
│  ├─ database
│  │  ├─ .env
│  │  ├─ docker-compose.yml
│  │  └─ init.sql
│  └─ LivinParis
│     ├─ Controllers ???
│     ├─ data
│     │  ├─ metro
│     │  │  ├─ arcs.csv
│     │  │  ├─ correspondance.csv
│     │  │  └─ noeuds.csv
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
│     │  │  ├─ Node.cs
│     │  │  └─ Station.cs
│     │  └─ Order
│     │     ├─ Account.cs
│     │     ├─ Adress.cs
│     │     ├─ Chef.cs
│     │     ├─ Company.cs
│     │     ├─ Contains.cs
│     │     ├─ Customer.cs
│     │     ├─ Dish.cs
│     │     ├─ Individual.cs
│     │     ├─ Ingredient.cs
│     │     ├─ MenuProposal.cs
│     │     ├─ OrderLine.cs
│     │     ├─ Review.cs
│     │     └─ Transaction.cs
│     ├─ Profiles ???
│     │  ├─ Chef.cs
│     │  ├─ Company.cs
│     │  ├─ Customer.cs
│     │  ├─ Individual.cs
│     │  └─ Manager.cs
│     ├─ Program.cs
│     ├─ Services ???
│     └─ Using.cs
└─ tests
   └─ LivinParis.Tests
      ├─ EdgeTests.cs
      ├─ GraphTests.cs
      ├─ LivinParis.Tests.csproj
      ├─ MSTestSettings.cs
      ├─ NodeTests.cs
      └─ Using.cs
