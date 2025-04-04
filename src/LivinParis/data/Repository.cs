using Castle.DynamicProxy;
using LivinParisRoussilleTeynier.Data.Services;

namespace LivinParisRoussilleTeynier.Data;

//TODO: visu

//TODO: peuplement de la base de données

//TODO: unit tests

public static class Repository
{
    #region Fields

    private static readonly ProxyGenerator s_generator = new();
    private static readonly ConnectionInterceptor s_interceptor = new();

    private static readonly AccountService s_accountService =
        s_generator.CreateClassProxy<AccountService>(s_interceptor);
    private static readonly AddressService s_addressService =
        s_generator.CreateClassProxy<AddressService>(s_interceptor);
    private static readonly ChefService s_chefService = s_generator.CreateClassProxy<ChefService>(
        s_interceptor
    );
    private static readonly CompanyService s_companyService =
        s_generator.CreateClassProxy<CompanyService>(s_interceptor);
    private static readonly ContainsService s_containsService =
        s_generator.CreateClassProxy<ContainsService>(s_interceptor);
    private static readonly CustomerService s_customerService =
        s_generator.CreateClassProxy<CustomerService>(s_interceptor);
    private static readonly DishService s_dishService = s_generator.CreateClassProxy<DishService>(
        s_interceptor
    );
    private static readonly IndividualService s_individualService =
        s_generator.CreateClassProxy<IndividualService>(s_interceptor);
    private static readonly IngredientService s_ingredientService =
        s_generator.CreateClassProxy<IngredientService>(s_interceptor);
    private static readonly MenuProposalService s_menuProposalService =
        s_generator.CreateClassProxy<MenuProposalService>(s_interceptor);
    private static readonly OrderLineService s_orderLineService =
        s_generator.CreateClassProxy<OrderLineService>(s_interceptor);
    private static readonly ReviewService s_reviewService =
        s_generator.CreateClassProxy<ReviewService>(s_interceptor);
    private static readonly TransactionService s_transactionService =
        s_generator.CreateClassProxy<TransactionService>(s_interceptor);

    #endregion Fields

    #region Properties

    public static AccountService Account
    {
        get { return s_accountService; }
    }
    public static AddressService Address
    {
        get { return s_addressService; }
    }
    public static ChefService Chef
    {
        get { return s_chefService; }
    }
    public static CompanyService Company
    {
        get { return s_companyService; }
    }
    public static ContainsService Contains
    {
        get { return s_containsService; }
    }
    public static CustomerService Customer
    {
        get { return s_customerService; }
    }
    public static DishService Dish
    {
        get { return s_dishService; }
    }
    public static IndividualService Individual
    {
        get { return s_individualService; }
    }
    public static IngredientService Ingredient
    {
        get { return s_ingredientService; }
    }
    public static MenuProposalService MenuProposal
    {
        get { return s_menuProposalService; }
    }
    public static OrderLineService OrderLine
    {
        get { return s_orderLineService; }
    }
    public static ReviewService Review
    {
        get { return s_reviewService; }
    }
    public static TransactionService Transaction
    {
        get { return s_transactionService; }
    }

    #endregion Properties
}
