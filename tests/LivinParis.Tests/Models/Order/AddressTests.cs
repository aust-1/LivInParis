namespace LivInParisRoussilleTeynier.Tests.Models.Order
{
    [TestClass]
    public class AddressTests
    {
        [TestMethod]
        public void Address_MissingNumberOrStreet_ShouldFail()
        {
            var a1 = new Address { AddressNumber = 0, Street = "rue A" };
            var a2 = new Address { AddressNumber = 10, Street = "" };

            Assert.IsTrue(
                ValidationHelper
                    .Validate(a1)
                    .Any(r => r.MemberNames.Contains(nameof(Address.AddressNumber)))
            );
            Assert.IsTrue(
                ValidationHelper
                    .Validate(a2)
                    .Any(r => r.MemberNames.Contains(nameof(Address.Street)))
            );
        }

        [TestMethod]
        public void Address_StreetTooLong_ShouldFail()
        {
            var addr = new Address { AddressNumber = 1, Street = new string('x', 51) };
            var results = ValidationHelper.Validate(addr);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(Address.Street))));
        }

        [TestMethod]
        public void Address_Valid_ShouldPass()
        {
            var addr = new Address { AddressNumber = 5, Street = "rue de Rivoli" };
            var results = ValidationHelper.Validate(addr);
            Assert.AreEqual(0, results.Count);
        }
    }
}
