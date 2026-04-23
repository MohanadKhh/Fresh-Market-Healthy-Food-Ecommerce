using ECommerce.DAL;
using FluentValidation;

namespace ECommerce.BLL
{
    public class CategoryWriteDtoValidator : AbstractValidator<CategoryWriteDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryWriteDtoValidator(IUnitOfWork unitOfWork)
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
        }

        private async Task<bool> CheckNameIsUniqueAsync(string name, CancellationToken cancellationToken)
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            return !categories.Any(p => p.Name == name);
        }
    }
}
