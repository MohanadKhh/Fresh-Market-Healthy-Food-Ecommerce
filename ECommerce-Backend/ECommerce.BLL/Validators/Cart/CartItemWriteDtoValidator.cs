using FluentValidation;

namespace ECommerce.BLL
{
    public class CartItemWriteDtoValidator : AbstractValidator<CartItemWriteDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartItemWriteDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(oi => oi.ProductId)
                .NotEmpty()
                .WithMessage("Product Id is required.")
                .WithErrorCode("ERR-01")

                .MustAsync(CheckProductExistence)
                .WithMessage("There is one product or more in order doesn't exist")
                .WithErrorCode("ERR-02");

            RuleFor(oi => oi.Quantity)
                .NotEmpty()
                .WithMessage("Quantity is required.")
                .WithErrorCode("ERR-03")

                .GreaterThanOrEqualTo(0)
                .WithMessage("Quantity must be 1 or greater.")
                .WithErrorCode("ERR-04");
        }

        private async Task<bool> CheckProductExistence(int productId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.ProductRepository.GetByIdAsync(productId) != null;
        }
    }
}
