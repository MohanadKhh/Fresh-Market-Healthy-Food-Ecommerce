

using System.Text.Json.Serialization;

namespace ECommerce.DAL
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        [JsonIgnore]
        public virtual Cart? Cart { get; set; }
        public int ProductId { get; set; }
        [JsonIgnore]
        public virtual Product? Product { get; set; }  // navigate to get live price
        public int Quantity { get; set; }
    }
}
