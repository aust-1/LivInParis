namespace LivInParisRoussilleTeynier.Tests.Models.Order
{
    [TestClass]
    public class DishTests
    {
        private static Dish CreateBaseDish() =>
            new()
            {
                DishName = "Plats",
                DishType = DishType.MainCourse,
                ExpiryTime = 48,
                CuisineNationality = "française",
                Quantity = 10,
                Price = 5.5m,
                ProductsOrigin = ProductsOrigin.France,
            };

        [TestMethod]
        public void Dish_NegativeQuantity_ShouldFail()
        {
            var d = CreateBaseDish();
            d.Quantity = -1;

            var results = ValidationHelper.Validate(d);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(Dish.Quantity))));
        }

        [TestMethod]
        public void Dish_NegativePrice_ShouldFail()
        {
            var d = CreateBaseDish();
            d.Price = -0.1m;

            var results = ValidationHelper.Validate(d);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(Dish.Price))));
        }

        [TestMethod]
        public void Dish_Valid_ShouldPass()
        {
            var d = CreateBaseDish();
            var results = ValidationHelper.Validate(d);
            Assert.AreEqual(0, results.Count);
        }
    }
}
