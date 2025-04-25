using System.ComponentModel.DataAnnotations;

namespace LivInParisRoussilleTeynier.Tests.Models.Order
{
    internal static class ValidationHelper
    {
        public static IList<ValidationResult> Validate(object model)
        {
            var ctx = new ValidationContext(model);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, ctx, results, validateAllProperties: true);
            return results;
        }
    }
}
