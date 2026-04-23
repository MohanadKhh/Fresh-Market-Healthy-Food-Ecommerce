using ECommerce.DAL;
using System.Text.Json.Serialization;

namespace ECommerce.BLL
{
    public static class OrderMapper
    {
        public static Order ToOrderModel(this OrderWriteDto orderWriteDto) => new()
        {
            Status = orderWriteDto.Status,
            CreatedAt = orderWriteDto.CreatedAt,
            OrderItems = (orderWriteDto.OrderItems ?? []).Select(oi => oi.ToOrderItemModel()).ToList(),
        };

        public static OrderItem ToOrderItemModel(this OrderItemWriteDto orderItemWriteDto) => new()
        {
            ProductId = orderItemWriteDto.ProductId,
            Price = orderItemWriteDto.Price,
            Quantity = orderItemWriteDto.Quantity,
        };

        public static OrderReadDto ToReadDto(this Order order) => new()
        {
            Id = order.Id,
            UserId = order.UserId,
            TotalPrice = order.TotalPrice,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            OrderItems = (order.OrderItems ?? []).Select(oi => oi.ToReadDto()).ToList(),
        };

        public static OrderItemReadDto ToReadDto(this OrderItem orderItem) => new()
        {
            Id = orderItem.Id,
            OrderId = orderItem.OrderId,
            Price = orderItem.Price,
            Quantity = orderItem.Quantity,
            Product = orderItem.Product is null ? null : orderItem.Product.ToReadDto()
        };
    }
}
