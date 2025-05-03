using DotNetEnv;
using LivInParisRoussilleTeynier.Models.Order;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Data
{
    /// <summary>
    /// EF Core database context for the Liv’in Paris application.
    /// </summary>
    public class LivInParisContext : DbContext
    {
        /// <summary>
        /// Configures the database connection.
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Env.Load(Path.Combine("..", "database", ".env"));

            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");

            var connectionString =
                $"SERVER={dbHost};PORT={dbPort};DATABASE={dbName};UID={dbUser};PASSWORD={Environment.GetEnvironmentVariable("DB_PASSWORD")}";
            optionsBuilder.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 32))
            );
        }

        /// <summary>
        /// Accounts table.
        /// </summary>
        public DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// Addresses table.
        /// </summary>
        public DbSet<Address> Addresses { get; set; }

        /// <summary>
        /// Chefs table.
        /// </summary>
        public DbSet<Chef> Chefs { get; set; }

        /// <summary>
        /// Companies table.
        /// </summary>
        public DbSet<Company> Companies { get; set; }

        /// <summary>
        /// Contains (join) table.
        /// </summary>
        public DbSet<Contains> Contains { get; set; }

        /// <summary>
        /// Customers table.
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// Dishes table.
        /// </summary>
        public DbSet<Dish> Dishes { get; set; }

        /// <summary>
        /// Ingredients table.
        /// </summary>
        public DbSet<Ingredient> Ingredients { get; set; }

        /// <summary>
        /// Individuals table.
        /// </summary>
        public DbSet<Individual> Individuals { get; set; }

        /// <summary>
        /// MenuProposals table.
        /// </summary>
        public DbSet<MenuProposal> MenuProposals { get; set; }

        /// <summary>
        /// OrderLines table.
        /// </summary>
        public DbSet<OrderLine> OrderLines { get; set; }

        /// <summary>
        /// OrderTransactions table.
        /// </summary>
        public DbSet<OrderTransaction> OrderTransactions { get; set; }

        /// <summary>
        /// Reviews table.
        /// </summary>
        public DbSet<Review> Reviews { get; set; }

        /// <summary>
        /// Configures model constraints, indexes, keys and relationships.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Address>(e =>
            {
                e.ToTable("Address");
                e.HasKey(a => a.AddressId);
                e.Property(a => a.AddressNumber).IsRequired();
                e.HasCheckConstraint("CK_Address_Number_Positive", "`AddressNumber` > 0");
                e.Property(a => a.Street).IsRequired().HasMaxLength(100);
                e.Property(a => a.NearestStation).HasConversion<string>().HasMaxLength(50);

                e.HasIndex(x => new { x.AddressNumber, x.Street })
                    .IsUnique()
                    .HasDatabaseName("IX_Address_Number_Street");
            });

            modelBuilder.Entity<Account>(e =>
            {
                e.ToTable("Account");
                e.HasKey(a => a.AccountId);
                e.Property(a => a.AccountEmail).IsRequired().HasMaxLength(100);
                e.Property(a => a.AccountPassword).IsRequired().HasMaxLength(50);

                e.HasIndex(x => x.AccountEmail).IsUnique().HasDatabaseName("IX_Account_Email");
            });

            modelBuilder.Entity<Chef>(e =>
            {
                e.ToTable("Chef");
                e.HasKey(c => c.AccountId);
                e.Property(c => c.ChefIsBanned).IsRequired();
                e.Property(c => c.ChefRating).HasColumnType("decimal(2,1)");

                e.HasCheckConstraint(
                    "CK_Chef_Rating",
                    "`ChefRating` IS NULL OR (`ChefRating` BETWEEN 1.0 AND 5.0)"
                );

                e.HasOne<Account>()
                    .WithOne()
                    .HasForeignKey<Chef>(c => c.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(c => c.Address)
                    .WithMany(a => a.Chefs)
                    .HasForeignKey(c => c.AddressId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Customer>(e =>
            {
                e.ToTable("Customer");
                e.HasKey(c => c.AccountId);
                e.Property(c => c.CustomerIsBanned).IsRequired();
                e.Property(c => c.CustomerRating).HasColumnType("decimal(2,1)");

                e.HasCheckConstraint(
                    "CK_Customer_Rating",
                    "`CustomerRating` IS NULL OR (`CustomerRating` BETWEEN 1.0 AND 5.0)"
                );

                e.HasOne<Account>()
                    .WithOne()
                    .HasForeignKey<Customer>(c => c.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Company>(e =>
            {
                e.ToTable("Company");
                e.HasKey(c => c.AccountId);
                e.Property(c => c.CompanyName).IsRequired().HasMaxLength(50);
                e.Property(c => c.ContactFirstName).IsRequired(false).HasMaxLength(50);
                e.Property(c => c.ContactLastName).IsRequired(false).HasMaxLength(50);

                e.HasIndex(c => c.CompanyName).IsUnique().HasDatabaseName("IX_Company_CompanyName");

                e.HasOne<Customer>()
                    .WithOne()
                    .HasForeignKey<Company>(c => c.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Individual>(e =>
            {
                e.ToTable("Individual");
                e.HasKey(i => i.AccountId);
                e.Property(i => i.LastName).IsRequired().HasMaxLength(50);
                e.Property(i => i.FirstName).IsRequired().HasMaxLength(50);
                e.Property(i => i.PersonalEmail).IsRequired().HasMaxLength(100);
                e.Property(i => i.PhoneNumber).IsRequired().HasMaxLength(50);

                e.HasIndex(i => i.PhoneNumber)
                    .IsUnique()
                    .HasDatabaseName("IX_Individual_PhoneNumber");

                e.HasOne<Customer>()
                    .WithOne()
                    .HasForeignKey<Individual>(i => i.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(i => i.Address)
                    .WithMany(a => a.Individuals)
                    .HasForeignKey(i => i.AddressId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Dish>(e =>
            {
                e.ToTable("Dish");
                e.HasKey(d => d.DishId);
                e.Property(d => d.DishName).IsRequired().HasMaxLength(50);
                e.Property(d => d.DishType).IsRequired().HasConversion<string>().HasMaxLength(20);
                e.Property(d => d.ExpiryTime).IsRequired();
                e.Property(d => d.CuisineNationality)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);
                e.Property(d => d.Quantity).IsRequired();
                e.Property(d => d.Price).IsRequired().HasColumnType("decimal(10,2)").IsRequired();
                e.Property(d => d.ProductsOrigin)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(10);
                e.Property(d => d.PhotoPath).HasMaxLength(255);

                e.HasCheckConstraint("CK_Dish_Quantity_Positive", "`Quantity` >= 0");
                e.HasCheckConstraint("CK_Dish_Price_Positive", "`Price` >= 0");
            });

            modelBuilder.Entity<Ingredient>(e =>
            {
                e.ToTable("Ingredient");
                e.HasKey(i => i.IngredientId);
                e.Property(i => i.IngredientName).IsRequired().HasMaxLength(50);
                e.Property(i => i.IsVegetarian).IsRequired();
                e.Property(i => i.IsVegan).IsRequired();
                e.Property(i => i.IsGlutenFree).IsRequired();
                e.Property(i => i.IsLactoseFree).IsRequired();
                e.Property(i => i.IsHalal).IsRequired();
                e.Property(i => i.IsKosher).IsRequired();

                e.HasIndex(i => i.IngredientName).IsUnique().HasDatabaseName("IX_Ingredient_Name");
            });

            modelBuilder.Entity<Contains>(e =>
            {
                e.ToTable("Contains");
                e.HasKey(c => new { c.IngredientId, c.DishId });

                e.HasOne(c => c.Ingredient)
                    .WithMany(i => i.Contains)
                    .HasForeignKey(c => c.IngredientId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(c => c.Dish)
                    .WithMany(d => d.Contains)
                    .HasForeignKey(c => c.DishId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MenuProposal>(e =>
            {
                e.ToTable("MenuProposal");
                e.HasKey(mp => new { mp.AccountId, mp.ProposalDate });
                e.Property(mp => mp.ProposalDate).HasColumnType("date").IsRequired();

                e.HasOne(mp => mp.Chef)
                    .WithMany(c => c.MenuProposals)
                    .HasForeignKey(mp => mp.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(mp => mp.Dish)
                    .WithMany(d => d.MenuProposals)
                    .HasForeignKey(mp => mp.DishId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OrderTransaction>(e =>
            {
                e.ToTable("OrderTransaction");
                e.HasKey(ot => ot.TransactionId);
                e.Property(ot => ot.TransactionDatetime).IsRequired();

                e.HasOne(ot => ot.Customer)
                    .WithMany(c => c.OrderTransactions)
                    .HasForeignKey(ot => ot.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderLine>(e =>
            {
                e.ToTable("OrderLine");
                e.HasKey(ol => ol.OrderLineId);
                e.Property(ol => ol.OrderLineDatetime).IsRequired();
                e.Property(ol => ol.OrderLineStatus)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(20);

                e.HasOne(ol => ol.Address)
                    .WithMany(a => a.OrderLines)
                    .HasForeignKey(ol => ol.AddressId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(ol => ol.OrderTransaction)
                    .WithMany(ot => ot.OrderLines)
                    .HasForeignKey(ol => ol.TransactionId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(ol => ol.Chef)
                    .WithMany(c => c.OrderLines)
                    .HasForeignKey(ol => ol.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Review>(e =>
            {
                e.ToTable("Review");
                e.HasKey(r => r.ReviewId);
                e.Property(r => r.ReviewerType)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(20);
                e.Property(r => r.ReviewDate).HasColumnType("date").IsRequired();
                e.Property(r => r.ReviewRating).HasColumnType("decimal(2,1)");
                e.Property(r => r.Comment).HasMaxLength(500);

                e.HasCheckConstraint(
                    "CK_Review_Rating",
                    "`ReviewRating` IS NULL OR (`ReviewRating` BETWEEN 1.0 AND 5.0)"
                );

                e.HasOne(r => r.OrderLine)
                    .WithMany(ol => ol.Reviews)
                    .HasForeignKey(r => r.OrderLineId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
