using ECommerce.Common;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.BLL
{
    public class CartManager : ICartManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IValidator<CartItemWriteDto> _writeCartItemValidator;

        public CartManager(IUnitOfWork unitOfWork
            , UserManager<ApplicationUser> userManager
            , IValidator<CartItemWriteDto> writeCartItemValidator)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _writeCartItemValidator = writeCartItemValidator;
        }

        public async Task<GeneralResult<CartReadDto>> GetCartByUserIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return GeneralResult<CartReadDto>.NotFound("User not found");

            var cart = await _unitOfWork.CartRepository.GetWithCartItemsByUserIdAsync(userId);
            if (cart == null)
                return GeneralResult<CartReadDto>.NotFound("User doesn't have a cart");

            cart.CalculateTotalPrice();
            var cartReadDto = cart.ToReadDto();
            return GeneralResult<CartReadDto>.SuccessedResult(cartReadDto);
        }

        public async Task<GeneralResult<CartReadDto>> AddItemAsync(string userId, CartItemWriteDto cartItemWriteDto)
        {
            var cart = await _unitOfWork.CartRepository.GetWithCartItemsByUserIdAsync(userId);
            if (cart == null)
            {
                return GeneralResult<CartReadDto>.NotFound("Cart not found");
            }

            //Validation of Cart Item needed to add
            var validationCartItemRes = await _writeCartItemValidator.ValidateAsync(cartItemWriteDto);
            if (!validationCartItemRes.IsValid)
            {
                Dictionary<string, List<Error>> errors = validationCartItemRes.ToError();
                return GeneralResult<CartReadDto>.FailedResult(errors);
            }

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(cartItemWriteDto.ProductId);
            if(product.Stock < cartItemWriteDto.Quantity) 
            {
                return GeneralResult<CartReadDto>.FailedResult($"The stoct of product with id '{cartItemWriteDto.ProductId}' not enough");
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == cartItemWriteDto.ProductId);
            if (cartItem != null)
            {
                cartItem.Quantity = cartItemWriteDto.Quantity;
                await _unitOfWork.SaveAsync();

                cart.CalculateTotalPrice();
                var cartRead = cart.ToReadDto();
                return GeneralResult<CartReadDto>.SuccessedResult(cartRead, "Item is already in cart, so updated quantity only");
            }

            cartItem = cartItemWriteDto.ToCartItemModel();
            cart.CartItems.Add(cartItem);
            await _unitOfWork.SaveAsync();

            //cart = await _unitOfWork.CartRepository.GetWithCartItemsByUserIdAsync(userId);
            
            cart.CalculateTotalPrice();
            var cartReadDto = cart.ToReadDto();
            return GeneralResult<CartReadDto>.SuccessedResult(cartReadDto, "Item added sccessfully to Cart");
        }

        public async Task<GeneralResult<CartReadDto>> RemoveItemAsync(string userId, int productId)
        {
            var cart = await _unitOfWork.CartRepository.GetWithCartItemsByUserIdAsync(userId);

            if (cart == null)
            {
                return GeneralResult<CartReadDto>.NotFound("Cart not found");
            }
            else
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (cartItem == null)
                {
                    return GeneralResult<CartReadDto>.NotFound("Cart Item not found in that cart");
                }
                else
                {
                    cart.CartItems.Remove(cartItem);
                    await _unitOfWork.SaveAsync();

                    cart.CalculateTotalPrice();
                    var cartReadDto = cart.ToReadDto();
                    return GeneralResult<CartReadDto>.SuccessedResult(cartReadDto, "Item removed sccessfully from Cart");
                }
            }
        }

        public async Task<GeneralResult<CartReadDto>> EditCartItemQuantityAsync(string userId, CartItemWriteDto cartItemWriteDto)
        {
            var cart = await _unitOfWork.CartRepository.GetWithCartItemsByUserIdAsync(userId);

            if (cart == null)
            {
                return GeneralResult<CartReadDto>.NotFound("Cart not found");
            }

            //Validation of Cart Item needed to edit
            var validationCartItemRes = await _writeCartItemValidator.ValidateAsync(cartItemWriteDto);
            if (!validationCartItemRes.IsValid)
            {
                Dictionary<string, List<Error>> errors = validationCartItemRes.ToError();
                return GeneralResult<CartReadDto>.FailedResult(errors);
            }

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(cartItemWriteDto.ProductId);
            if (product.Stock < cartItemWriteDto.Quantity)
            {
                return GeneralResult<CartReadDto>.FailedResult($"The stoct of product with id '{cartItemWriteDto.ProductId}' not enough");
            }

            else
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == cartItemWriteDto.ProductId);
                if (cartItem == null)
                {
                    return GeneralResult<CartReadDto>.NotFound("Cart Item not found in that cart");
                }
                else
                {
                    cartItem.Quantity = cartItemWriteDto.Quantity;
                    await _unitOfWork.SaveAsync();

                    cart.CalculateTotalPrice();
                    var cartReadDto = cart.ToReadDto();
                    return GeneralResult<CartReadDto>.SuccessedResult(cartReadDto, "Item' quantity updated sccessfully");
                }
            }
        }
    }
}
