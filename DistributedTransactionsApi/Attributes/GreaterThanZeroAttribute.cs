using System.ComponentModel.DataAnnotations;

namespace DistributedTransactionsApi.Attributes;

public class GreaterThanZeroAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        return value switch
        {
            null => ValidationResult.Success,
            IComparable comparable when comparable.CompareTo(0.0) <= 0 => new ValidationResult(ErrorMessage ??
                "The field must be greater than zero."),
            _ => ValidationResult.Success
        };
    }
}