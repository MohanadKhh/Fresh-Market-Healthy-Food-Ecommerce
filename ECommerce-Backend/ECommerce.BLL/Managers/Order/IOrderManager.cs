namespace ECommerce.BLL
{
    public interface IOrderManager
    {
        Task<GeneralResult<IEnumerable<OrderReadDto>>> GetAllOrdersByUserIdAsync(string userId);
        Task<GeneralResult<OrderReadDto>> GetOrderByIdAsync(int id);
        Task<GeneralResult<OrderReadDto>> PlaceOrderAsync(string userId, OrderWriteDto orderWriteDto);
    }
}