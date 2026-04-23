using System.Text.Json.Serialization;

namespace ECommerce.DAL
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public decimal TotalPrice { get; private set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public void CalculateTotalPrice()
        {
            TotalPrice = OrderItems.Sum(i => i.Price * i.Quantity);
        }
    }
}
