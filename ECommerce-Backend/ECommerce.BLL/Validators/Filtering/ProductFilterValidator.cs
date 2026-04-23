using ECommerce.Common;
using FluentValidation;

namespace ECommerce.BLL
{
    public class ProductFilterValidator : AbstractValidator<ProductFilter>
    {
        public ProductFilterValidator()
        {
            RuleFor(f => f.MinPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("MinPrice must be greater than or equal to 0.")
                .WithErrorCode("ERR-01");

            RuleFor(f => f.MaxPrice)
                .GreaterThanOrEqualTo(1)
                .WithMessage("MaxPrice must be greater than or equal to 1.")
                .WithErrorCode("ERR-02")

                .LessThanOrEqualTo(1_000_000)
                .WithMessage("MaxPrice must be less than or equal to 1,000,000.")
                .WithErrorCode("ERR-03");

            RuleFor(f => f)
                .Must(f =>
                {
                    if (f.MinPrice.HasValue && f.MaxPrice.HasValue)
                    {
                        return f.MaxPrice >= f.MinPrice;
                    }
                    return true;
                })
                .WithMessage("MaxPrice must be greater than or equal to MinPrice.")
                .WithErrorCode("ERR-04");

            RuleFor(f => f.SortBy)
                .Must(sortBy => string.IsNullOrWhiteSpace(sortBy)
                                || sortBy.Equals("price", StringComparison.OrdinalIgnoreCase)
                                || sortBy.Equals("reviews", StringComparison.OrdinalIgnoreCase))
                .WithMessage("SortBy must be empty or 'price' or 'reviews'.")
                .WithErrorCode("ERR-05");
        }
    }
}
