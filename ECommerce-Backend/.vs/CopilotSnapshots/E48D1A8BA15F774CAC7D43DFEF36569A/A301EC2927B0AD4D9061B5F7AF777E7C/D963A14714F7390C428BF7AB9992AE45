using ECommerce.DAL;

namespace ECommerce.BLL
{
    public static class ProductMapper
    {
        public static ProductReadDto ToReadDTO(this Product product) => new()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Count = product.Count,
            ExpiryDate = product.ExpiryDate ?? new DateOnly(1, 1, 1),
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId,
            CategoryName = product.Category!.Name,
        };


        public static Product ToProductModel(this ProductWriteDto productWriteDTO) => new()
        {
            Name = productWriteDTO.Name,
            Description = productWriteDTO.Description,
            Price = productWriteDTO.Price,
            Count = productWriteDTO.Count,
            ImageUrl = productWriteDTO.ImageUrl,
            ExpiryDate = productWriteDTO.ExpiryDate,
            CategoryId = productWriteDTO.CategoryId
        };
    }
}
