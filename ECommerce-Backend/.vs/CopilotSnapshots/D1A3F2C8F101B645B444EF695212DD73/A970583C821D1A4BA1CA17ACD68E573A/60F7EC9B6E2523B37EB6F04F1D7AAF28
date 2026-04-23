using ECommerce.DAL;

namespace ECommerce.BLL
{
    public static class CartMapper
    {
        public static CartReadDto ToReadDto(this Cart cart) => new()
        {
            Id = cart.Id,
            UserId = cart.UserId,
            TotalPrice = cart.TotalPrice,
            CartItems = (cart.CartItems ?? []).Select(ci => ci.ToReadDto()).ToList(),
        };

        public static CartItemReadDto ToReadDto(this CartItem cartItem) => new()
        {
            Id = cartItem.Id,
            Quantity = cartItem.Quantity,
            Product = cartItem.Product is null ? null : cartItem.Product.ToReadDto()
        };

        public static CartItem ToCartItemModel(this CartItemWriteDto cartItemWriteDto) => new()
        {
            ProductId = cartItemWriteDto.ProductId,
            Quantity = cartItemWriteDto.Quantity,
        };
    }
}
