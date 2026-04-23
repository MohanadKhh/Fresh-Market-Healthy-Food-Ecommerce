namespace ECommerce.BLL
{
    public class ProductReadDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Unit { get; set; } = "kg";
        public int Reviews { get; set; }
        public bool IsOrganic { get; set; }
        public bool IsFeatured { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
