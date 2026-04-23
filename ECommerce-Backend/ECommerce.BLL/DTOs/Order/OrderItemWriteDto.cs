using ECommerce.DAL;
using System.Text.Json.Serialization;

namespace ECommerce.BLL
{
    public class OrderItemWriteDto
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
