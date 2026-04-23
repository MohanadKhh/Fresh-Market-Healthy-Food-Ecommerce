using ECommerce.Common;
using FluentValidation;

namespace ECommerce.BLL
{
    public class PaginationParametersValidator : AbstractValidator<PaginationParameters>
    {
        public PaginationParametersValidator()
        {
            RuleFor(p => p.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0.")
                .WithErrorCode("ERR-01");

            RuleFor(p => p.PageSize)
                .InclusiveBetween(1, 50)
                .WithMessage("PageSize must be between 1 and 50.")
                .WithErrorCode("ERR-02");
        }
    }
}
