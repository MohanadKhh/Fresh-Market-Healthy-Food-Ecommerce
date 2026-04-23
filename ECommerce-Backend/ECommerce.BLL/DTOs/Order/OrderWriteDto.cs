using ECommerce.DAL;
using System.Text.Json.Serialization;

namespace ECommerce.BLL
{
    public class OrderWriteDto
    {
        public ICollection<OrderItemWriteDto> OrderItems { get; set; } = new HashSet<OrderItemWriteDto>();
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
