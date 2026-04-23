using ECommerce.DAL;

namespace ECommerce.BLL
{
    public class OrderReadDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<OrderItemReadDto> OrderItems { get; set; } = new HashSet<OrderItemReadDto>();
    }
}
