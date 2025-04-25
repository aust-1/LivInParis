namespace LivInParisRoussilleTeynier.Tests.Models.Order
{
    [TestClass]
    public class IngredientTests
    {
        private Ingredient CreateBase() =>
            new Ingredient
            {
                IngredientName = "Tomate",
                IsVegetarian = true,
                IsVegan = true,
                IsGlutenFree = true,
                IsLactoseFree = true,
                IsHalal = false,
                IsKosher = false,
            };

        [TestMethod]
        public void Ingredient_NameTooLong_ShouldFail()
        {
            var i = CreateBase();
            i.IngredientName = new string('x', 51);
            var results = ValidationHelper.Validate(i);
            Assert.IsTrue(
                results.Any(r => r.MemberNames.Contains(nameof(Ingredient.IngredientName)))
            );
        }

        [TestMethod]
        public void Ingredient_DefaultBooleans_ShouldBeFalseOrTrue()
        {
            var i = CreateBase();
            // on vérifie juste que la valeur placée est bien prise en compte
            Assert.IsTrue(i.IsVegetarian);
            Assert.IsTrue(i.IsGlutenFree);
        }

        [TestMethod]
        public void Ingredient_Valid_ShouldPass()
        {
            var i = CreateBase();
            var results = ValidationHelper.Validate(i);
            Assert.AreEqual(0, results.Count);
        }
    }
}
