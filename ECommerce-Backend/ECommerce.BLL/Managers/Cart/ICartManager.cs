using ECommerce.Common;

namespace ECommerce.BLL
{
    public interface ICartManager
    {
        Task<GeneralResult<CartReadDto>> AddItemAsync(string userId, CartItemWriteDto cartItemWriteDto);
        Task<GeneralResult<CartReadDto>> EditCartItemQuantityAsync(string userId, CartItemWriteDto cartItemWriteDto);
        Task<GeneralResult<CartReadDto>> GetCartByUserIdAsync(string userId);
        Task<GeneralResult<CartReadDto>> RemoveItemAsync(string userId, int productId);
    }
}