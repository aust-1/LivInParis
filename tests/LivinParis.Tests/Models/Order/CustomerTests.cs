namespace LivInParisRoussilleTeynier.Tests.Models.Order
{
    [TestClass]
    public class CustomerTests
    {
        static readonly Account a1 = new()
        {
            AccountEmail = "test@gmail.com",
            AccountPassword = "password",
        };

        static readonly Account a2 = new()
        {
            AccountEmail = "test2@gmail.com",
            AccountPassword = "password",
        };

        [TestMethod]
        public void Customer_RatingOutOfRange_ShouldFail()
        {
            var c1 = new Customer
            {
                Account = a1,
                CustomerIsBanned = false,
                CustomerRating = 0m,
            };
            var c2 = new Customer
            {
                Account = a1,
                CustomerIsBanned = false,
                CustomerRating = 6m,
            };

            Assert.IsTrue(
                ValidationHelper
                    .Validate(c1)
                    .Any(r => r.MemberNames.Contains(nameof(Customer.CustomerRating)))
            );
            Assert.IsTrue(
                ValidationHelper
                    .Validate(c2)
                    .Any(r => r.MemberNames.Contains(nameof(Customer.CustomerRating)))
            );
        }

        [TestMethod]
        public void Customer_MissingBanFlag_ShouldHaveFalseBanFlag()
        {
            var c = new Customer { Account = a1 };
            Assert.IsFalse(c.CustomerIsBanned);
        }

        [TestMethod]
        public void Customer_Valid_ShouldPass()
        {
            var c = new Customer
            {
                CustomerIsBanned = false,
                Account = new Account { AccountEmail = "a@b.com", AccountPassword = "pwd" },
            };
            var results = ValidationHelper.Validate(c);
            Assert.AreEqual(0, results.Count);
        }
    }
}
