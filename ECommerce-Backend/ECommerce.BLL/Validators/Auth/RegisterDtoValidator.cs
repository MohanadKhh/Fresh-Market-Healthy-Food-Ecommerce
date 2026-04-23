using FluentValidation;

namespace ECommerce.BLL
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(r => r.FirstName)
                .NotEmpty()
                .WithMessage("First name is required.")
                .WithErrorCode("ERR-01")
                .MinimumLength(2)
                .WithMessage("First name must be at least 2 characters.")
                .WithErrorCode("ERR-02");

            RuleFor(r => r.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.")
                .WithErrorCode("ERR-03")
                .MinimumLength(2)
                .WithMessage("Last name must be at least 2 characters.")
                .WithErrorCode("ERR-04");

            RuleFor(r => r.UserName)
                .NotEmpty()
                .WithMessage("Username is required.")
                .WithErrorCode("ERR-05")
                .MinimumLength(3)
                .WithMessage("Username must be at least 3 characters.")
                .WithErrorCode("ERR-06");

            RuleFor(r => r.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .WithErrorCode("ERR-07")
                .EmailAddress()
                .WithMessage("Email format is invalid.")
                .WithErrorCode("ERR-08");

            RuleFor(r => r.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .WithErrorCode("ERR-09")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters.")
                .WithErrorCode("ERR-10");

            RuleFor(r => r.ConfirmPassword)
                .Equal(r => r.Password)
                .WithMessage("Confirm password must match password.")
                .WithErrorCode("ERR-11");
        }
    }
}
