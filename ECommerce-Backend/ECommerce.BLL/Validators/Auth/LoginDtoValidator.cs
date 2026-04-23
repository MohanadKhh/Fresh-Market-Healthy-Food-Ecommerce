using FluentValidation;

namespace ECommerce.BLL
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(l => l.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .WithErrorCode("ERR-01")
                .EmailAddress()
                .WithMessage("Email format is invalid.")
                .WithErrorCode("ERR-02");

            RuleFor(l => l.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .WithErrorCode("ERR-03");
        }
    }
}
