using System.Globalization;
using Aspose.Cells;
using Microsoft.Extensions.DependencyInjection;

namespace LivInParisRoussilleTeynier;

/// <summary>
/// Provides methods to seed the database from an Excel workbook.
/// </summary>
public static class DatabaseSeeder
{
    /// <summary>
    /// Reads data from the specified Excel file and populates the database.
    /// </summary>
    /// <param name="dataDirectory">Directory containing the workbook.</param>
    /// <param name="serviceProvider">Service provider to resolve the DbContext.</param>
    public static async Task SeedFromExcelAsync(
        string dataDirectory,
        IServiceProvider serviceProvider
    )
    {
        var filePath = Path.Combine(dataDirectory, "Peuplement.xlsx");
        var workbook = new Workbook(filePath);

        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LivInParisContext>();

        var addressCells = workbook.Worksheets[0].Cells;
        var accountCells = workbook.Worksheets[1].Cells;
        var chefCells = workbook.Worksheets[2].Cells;
        var companyCells = workbook.Worksheets[3].Cells;
        var containsCells = workbook.Worksheets[4].Cells;
        var customerCells = workbook.Worksheets[5].Cells;
        var dishCells = workbook.Worksheets[6].Cells;
        var individualCells = workbook.Worksheets[7].Cells;
        var ingredientCells = workbook.Worksheets[8].Cells;
        var menuProposalCells = workbook.Worksheets[9].Cells;
        var orderLineCells = workbook.Worksheets[10].Cells;
        var orderTransactionCells = workbook.Worksheets[11].Cells;
        var reviewCells = workbook.Worksheets[12].Cells;

        await SeedAddressesAsync(addressCells, context);
        await SeedAccountsAsync(accountCells, context);
        await SeedCustomersAsync(customerCells, context);
        await SeedChefsAsync(chefCells, context);
        await SeedCompaniesAsync(companyCells, context);
        await SeedIndividualsAsync(individualCells, context);
        await SeedDishesAsync(dishCells, context);
        await SeedIngredientsAsync(ingredientCells, context);
        await SeedContainsAsync(containsCells, context);
        await SeedMenuProposalsAsync(menuProposalCells, context);
        await SeedOrderTransactionsAsync(orderTransactionCells, context);
        await SeedOrderLinesAsync(orderLineCells, context);
        await SeedReviewsAsync(reviewCells, context);

        await context.SaveChangesAsync();
    }

    private static Task SeedAddressesAsync(Cells cells, LivInParisContext context)
    {
        for (var row = 1; row < cells.MaxDataRow + 1; row++)
        {
            var addressNumber = cells[row, 0].IntValue;
            var street = cells[row, 1].StringValue;
            var nearestStation = Metro.GetNearestStation($"{addressNumber} {street}").Result;

            var address = new Address
            {
                AddressNumber = addressNumber,
                Street = street,
                NearestStation = nearestStation,
            };
            context.Addresses.Add(address);
        }
        return Task.CompletedTask;
    }

    private static Task SeedAccountsAsync(Cells cells, LivInParisContext context)
    {
        for (var row = 1; row < cells.MaxDataRow + 1; row++)
        {
            var account = new Account
            {
                AccountEmail = cells[row, 0].StringValue,
                AccountPassword = cells[row, 1].StringValue,
            };
            context.Accounts.Add(account);
        }
        return Task.CompletedTask;
    }

    private static Task SeedCustomersAsync(Cells cells, LivInParisContext context)
    {
        for (var row = 1; row < cells.MaxDataRow + 1; row++)
        {
            var customer = new Customer
            {
                AccountId = cells[row, 0].IntValue,
                CustomerRating = (decimal?)cells[row, 1].DoubleValue,
                CustomerIsBanned = cells[row, 2].BoolValue,
            };
            context.Customers.Add(customer);
        }
        return Task.CompletedTask;
    }

    private static Task SeedChefsAsync(Cells cells, LivInParisContext context)
    {
        for (var row = 1; row < cells.MaxDataRow + 1; row++)
        {
            var chef = new Chef
            {
                AccountId = cells[row, 0].IntValue,
                ChefRating = (decimal?)cells[row, 1].DoubleValue,
                ChefIsBanned = cells[row, 2].BoolValue,
                AddressId = cells[row, 3].IntValue,
            };
            context.Chefs.Add(chef);
        }
        return Task.CompletedTask;
    }

    private static Task SeedCompaniesAsync(Cells cells, LivInParisContext context)
    {
        for (var row = 1; row < cells.MaxDataRow + 1; row++)
        {
            var company = new Company
            {
                AccountId = cells[row, 0].IntValue,
                CompanyName = cells[row, 1].StringValue,
                ContactFirstName = cells[row, 2].StringValue,
                ContactLastName = cells[row, 3].StringValue,
            };
            context.Companies.Add(company);
        }
        return Task.CompletedTask;
    }

    private static Task SeedIndividualsAsync(Cells cells, LivInParisContext context)
    {
        for (var row = 1; row < cells.MaxDataRow + 1; row++)
        {
            var individual = new Individual
            {
                AccountId = cells[row, 0].IntValue,
                LastName = cells[row, 1].StringValue,
                FirstName = cells[row, 2].StringValue,
                PersonalEmail = cells[row, 3].StringValue,
                PhoneNumber = cells[row, 4].StringValue,
                AddressId = cells[row, 5].IntValue,
            };
            context.Individuals.Add(individual);
        }
        return Task.CompletedTask;
    }

    private static Task SeedDishesAsync(Cells cells, LivInParisContext context)
    {
        for (var row = 1; row < cells.MaxDataRow + 1; row++)
        {
            var dish = new Dish
            {
                DishName = cells[row, 0].StringValue,
                DishType = Enum.Parse<DishType>(cells[row, 1].StringValue),
                ExpiryTime = cells[row, 2].IntValue,
                CuisineNationality = cells[row, 3].StringValue,
                Quantity = cells[row, 4].IntValue,
                Price = (decimal)cells[row, 5].DoubleValue,
                ProductsOrigin = Enum.Parse<ProductsOrigin>(cells[row, 6].StringValue),
                PhotoPath = cells[row, 7].StringValue ?? "null.jpeg",
            };
            context.Dishes.Add(dish);
        }
        return Task.CompletedTask;
    }

    private static Task SeedIngredientsAsync(Cells cells, LivInParisContext context)
    {
        for (var row = 1; row < cells.MaxDataRow + 1; row++)
        {
            var ingredient = new Ingredient
            {
                IngredientName = cells[row, 0].StringValue,
                IsVegetarian = cells[row, 1].BoolValue,
                IsVegan = cells[row, 2].BoolValue,
                IsGlutenFree = cells[row, 3].BoolValue,
                IsLactoseFree = cells[row, 4].BoolValue,
                IsHalal = cells[row, 5].BoolValue,
                IsKosher = cells[row, 6].BoolValue,
            };
            context.Ingredients.Add(ingredient);
        }
        return Task.CompletedTask;
    }

    private static Task SeedContainsAsync(Cells cells, LivInParisContext context)
    {
        for (var row = 1; row < cells.MaxDataRow + 1; row++)
        {
            var relation = new Contains
            {
                IngredientId = cells[row, 0].IntValue,
                DishId = cells[row, 1].IntValue,
            };
            context.Contains.Add(relation);
        }
        return Task.CompletedTask;
    }

    private static Task SeedMenuProposalsAsync(Cells cells, LivInParisContext context)
    {
        for (var row = 1; row < cells.MaxDataRow + 1; row++)
        {
            var mp = new MenuProposal
            {
                AccountId = cells[row, 0].IntValue,
                ProposalDate = DateOnly.FromDateTime(
                    DateTime.Parse(cells[row, 1].StringValue, CultureInfo.InvariantCulture)
                ),
                DishId = cells[row, 2].IntValue,
            };
            context.MenuProposals.Add(mp);
        }
        return Task.CompletedTask;
    }

    private static Task SeedOrderTransactionsAsync(Cells cells, LivInParisContext context)
    {
        for (var row = 1; row < cells.MaxDataRow + 1; row++)
        {
            var ot = new OrderTransaction
            {
                TransactionDatetime = DateTime.Parse(
                    cells[row, 0].StringValue,
                    CultureInfo.InvariantCulture
                ),
                AccountId = cells[row, 1].IntValue,
            };
            context.OrderTransactions.Add(ot);
        }
        return Task.CompletedTask;
    }

    private static Task SeedOrderLinesAsync(Cells cells, LivInParisContext context)
    {
        for (var row = 1; row < cells.MaxDataRow + 1; row++)
        {
            var ol = new OrderLine
            {
                OrderLineDatetime = DateTime.Parse(
                    cells[row, 0].StringValue,
                    CultureInfo.InvariantCulture
                ),
                OrderLineStatus = Enum.Parse<OrderLineStatus>(cells[row, 1].StringValue),
                AddressId = cells[row, 2].IntValue,
                TransactionId = cells[row, 3].IntValue,
                AccountId = cells[row, 4].IntValue,
            };
            context.OrderLines.Add(ol);
        }
        return Task.CompletedTask;
    }

    private static Task SeedReviewsAsync(Cells cells, LivInParisContext context)
    {
        for (var row = 1; row < cells.MaxDataRow + 1; row++)
        {
            var review = new Review
            {
                ReviewerType = Enum.Parse<ReviewerType>(cells[row, 0].StringValue),
                ReviewRating = (decimal?)cells[row, 1].DoubleValue,
                Comment = cells[row, 2].StringValue,
                ReviewDate = DateOnly.FromDateTime(
                    DateTime.Parse(cells[row, 3].StringValue, CultureInfo.InvariantCulture)
                ),
                OrderLineId = cells[row, 4].IntValue,
            };
            context.Reviews.Add(review);
        }
        return Task.CompletedTask;
    }
}
