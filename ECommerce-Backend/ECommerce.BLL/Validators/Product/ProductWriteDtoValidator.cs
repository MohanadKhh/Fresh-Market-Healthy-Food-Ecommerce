using ECommerce.DAL;
using FluentValidation;

namespace ECommerce.BLL
{
    public class ProductWriteDtoValidator : AbstractValidator<ProductWriteDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductWriteDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage("Name can't be empty.")
                .WithErrorCode("ERR-01")

                .MinimumLength(3)
                .WithMessage("Name must be at least 3 character.")
                .WithErrorCode("ERR-02")

                .MaximumLength(50)
                .WithMessage("Name can't be more than 100 character.")
                .WithErrorCode("ERR-03")

                .MustAsync(CheckNameIsUniqueAsync)
                .WithMessage("Name must be unique")
                .WithErrorCode("ERR-04");

            RuleFor(p => p.Description)
                .NotEmpty()
                .WithMessage("Description can't be empty.")
                .WithErrorCode("ERR-05");

            RuleFor(p => p.Stock)
                .NotEmpty()
                .WithMessage("Stock can't be empty.")
                .WithErrorCode("ERR-06")

                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock must be positive.")
                .WithErrorCode("ERR-07");

            RuleFor(p => p.Price)
                .NotEmpty()
                .WithMessage("Price can't be empty.")
                .WithErrorCode("ERR-08")

                .GreaterThan(0)
                .WithMessage("Price must be geater than zero.")
                .WithErrorCode("ERR-09");

            RuleFor(p => p.ExpiryDate)
                .NotEmpty()
                .WithMessage("ExpiryDate must be positive")
                .WithErrorCode("ERR-10")

                .GreaterThan(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("ExpiryDate must be in future")
                .WithErrorCode("ERR-11");

            RuleFor(c => c.ImageUrl)
                .MaximumLength(500)
                .WithMessage("Image path can't be more than 500 characters.")
                .WithErrorCode("ERR-07")
                .When(c => !string.IsNullOrWhiteSpace(c.ImageUrl));

            RuleFor(p => p.Unit)
                .NotEmpty()
                .WithMessage("Unit can't be empty.")
                .WithErrorCode("ERR-12")
                .MaximumLength(20)
                .WithMessage("Unit can't be more than 20 characters.")
                .WithErrorCode("ERR-13");

            RuleFor(p => p.Reviews)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Reviews can't be negative.")
                .WithErrorCode("ERR-14");

            RuleFor(p => p.CreatedAt)
                .Must(createdAt => createdAt == default || createdAt <= DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("CreatedAt cannot be in the future.")
                .WithErrorCode("ERR-15");

            RuleFor(p => p.CategoryId)
                .MustAsync(CheckCategoryExistenceAsync)
                .WithMessage("That's Category Id doesn't exist")
                .WithErrorCode("ERR-16");
        }

        private async Task<bool> CheckNameIsUniqueAsync(string name, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            return !products.Any(p => p.Name == name);
        }

        private async Task<bool> CheckCategoryExistenceAsync(int categoryId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId) != null;
        }
    }
}
