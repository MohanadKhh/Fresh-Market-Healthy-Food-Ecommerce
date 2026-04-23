using ECommerce.DAL;

namespace ECommerce.BLL
{
    public class CartItemReadDto
    {
        public int Id { get; set; }
        public ProductReadDto? Product { get; set; }
        public int Quantity { get; set; }
    }
}
