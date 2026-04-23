
using System.Text.Json.Serialization;

namespace ECommerce.DAL
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        [JsonIgnore]
        public virtual Order? Order { get; set; }

        public int ProductId { get; set; }
        [JsonIgnore]
        public virtual Product? Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}