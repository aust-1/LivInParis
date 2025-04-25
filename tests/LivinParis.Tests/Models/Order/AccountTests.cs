namespace LivInParisRoussilleTeynier.Tests.Models.Order
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void Account_WithoutEmail_ShouldFailValidation()
        {
            var acct = new Account { AccountEmail = "", AccountPassword = "azerty" };

            var results = ValidationHelper.Validate(acct);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(Account.AccountEmail))));
        }

        [TestMethod]
        public void Account_EmailTooLong_ShouldFailValidation()
        {
            var acct = new Account
            {
                AccountEmail = new string('a', 101) + "@x.com",
                AccountPassword = "pwd",
            };

            var results = ValidationHelper.Validate(acct);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(Account.AccountEmail))));
        }

        [TestMethod]
        public void Account_Valid_ShouldPassValidation()
        {
            var acct = new Account { AccountEmail = "toto@example.com", AccountPassword = "pwd" };

            var results = ValidationHelper.Validate(acct);
            Assert.AreEqual(0, results.Count);
        }
    }
}
