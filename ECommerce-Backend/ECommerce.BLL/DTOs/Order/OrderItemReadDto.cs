using ECommerce.DAL;
using System.Text.Json.Serialization;

namespace ECommerce.BLL
{
    public class OrderItemReadDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public ProductReadDto? Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}