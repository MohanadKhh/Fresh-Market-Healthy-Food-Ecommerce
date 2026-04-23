using Microsoft.AspNetCore.Http;

namespace ECommerce.BLL
{
    public sealed record ImageUploadDto(IFormFile ImageFile);
}
