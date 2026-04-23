using ECommerce.DAL;

namespace ECommerce.BLL
{
    public static class CategoryMapper
    {
        public static CategoryReadDto ToReadDto(this Category category) => new()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ImageUrl = category.ImageUrl,
            ProductCount = category.ProductCount,
            Products = category.Products?.Select(p => p.ToReadDto())
        };

        public static Category ToCategoryModel(this CategoryWriteDto categoryWriteDto) => new()
        {
            Name = categoryWriteDto.Name,
            Description = categoryWriteDto.Description,
            ImageUrl = categoryWriteDto.ImageUrl
        };
    }
}
