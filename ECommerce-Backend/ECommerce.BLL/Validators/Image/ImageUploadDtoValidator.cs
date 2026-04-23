using FluentValidation;

namespace ECommerce.BLL
{
    public class ImageUploadDtoValidator : AbstractValidator<ImageUploadDto>
    {

        private static readonly string[] AllowedExtenstions = [".jpg", ".png", ".jpeg"];
        public ImageUploadDtoValidator()
        {
            RuleFor(i => i.ImageFile)
                .NotEmpty()
                .WithMessage("Image file is requried")
                .WithErrorCode("ERR-01");

            RuleFor(i => i.ImageFile.Length)
                .GreaterThan(0)
                .WithMessage("Image must nit be empty")
                .WithErrorCode("ERR-02")

                .LessThanOrEqualTo(5_000_000)
                .WithMessage("Image must be less that 5MB")
                .WithErrorCode("ERR-03");

            RuleFor(i => Path.GetExtension(i.ImageFile.FileName).ToLower())
                .Must(ext => AllowedExtenstions.Contains(ext))
                .WithMessage("Image must be with extention (jpg, png, jpeg)")
                .WithErrorCode("ERR-04");
        }
    }
}
