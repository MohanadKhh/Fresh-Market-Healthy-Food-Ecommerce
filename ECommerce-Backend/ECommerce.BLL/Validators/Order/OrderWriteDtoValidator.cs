using ECommerce.DAL;
using FluentValidation;

namespace ECommerce.BLL
{
    public class OrderWriteDtoValidator : AbstractValidator<OrderWriteDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderWriteDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(oi => oi.OrderItems)
                .NotEmpty()
                .WithMessage("Order must has at least one item.")
                .WithErrorCode("ERR-03");

            RuleFor(o => o.Status)
                .IsInEnum()
                .WithMessage("Invalid order status.")
                .WithErrorCode("ERR-04");

            RuleFor(o => o.Status)
                .Equal(OrderStatus.Pending)
                .WithMessage("New orders must start with Pending status.")
                .WithErrorCode("ERR-05");

            RuleFor(o => o.CreatedAt)
                .Must(createdAt => createdAt <= DateTime.UtcNow.AddSeconds(1))
                .WithMessage("CreatedAt cannot be in the future.")
                .WithErrorCode("ERR-06");

        }
    }
}
