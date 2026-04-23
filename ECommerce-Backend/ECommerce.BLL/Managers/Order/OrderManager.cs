using ECommerce.Common;
using FluentValidation;

namespace ECommerce.BLL
{
    public class OrderManager : IOrderManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<OrderWriteDto> _writeOrderValidator;
        private readonly IValidator<OrderItemWriteDto> _writeOrderItemValidator;

        public OrderManager(IUnitOfWork unitOfWork
            , IValidator<OrderWriteDto> writeOrderValidator
            , IValidator<OrderItemWriteDto> writeOrderItemValidator)
        {
            _unitOfWork = unitOfWork;
            _writeOrderValidator = writeOrderValidator;
            _writeOrderItemValidator = writeOrderItemValidator;
        }

        public async Task<GeneralResult<OrderReadDto>> PlaceOrderAsync(string userId, OrderWriteDto orderWriteDto)
        {
            //Validation of order
            var validationOrderRes = await _writeOrderValidator.ValidateAsync(orderWriteDto);
            if (!validationOrderRes.IsValid)
            {
                Dictionary<string, List<Error>> errors = validationOrderRes.ToError();
                return GeneralResult<OrderReadDto>.FailedResult(errors);
            }

            //Validation on each order item
            foreach (var oi in orderWriteDto.OrderItems)
            {
                var validationOrderItemRes = await _writeOrderItemValidator.ValidateAsync(oi);
                if (!validationOrderItemRes.IsValid)
                {
                    Dictionary<string, List<Error>> errors = validationOrderItemRes.ToError();
                    return GeneralResult<OrderReadDto>.FailedResult(errors, $"Validation error on Product Item with id '{oi.ProductId}'");
                }

                var productUpdated = await _unitOfWork.ProductRepository.GetByIdAsync(oi.ProductId);
                if (productUpdated.Stock < oi.Quantity)
                {
                    return GeneralResult<OrderReadDto>.FailedResult($"The stoct of product with id '{oi.ProductId}' not enough to place the order");
                }
            }

            var order = orderWriteDto.ToOrderModel();
            order.UserId = userId;
            order.CalculateTotalPrice();
            _unitOfWork.OrderRepository.Add(order);

            foreach (var oi in orderWriteDto.OrderItems)
            {
                var productUpdated = await _unitOfWork.ProductRepository.GetByIdAsync(oi.ProductId);
                productUpdated.Stock -= oi.Quantity;
            }

            await _unitOfWork.SaveAsync();

            //That to get order and all dependent entites of it to show
            order = await _unitOfWork.OrderRepository.GetByIdWithItemsAsync(order.Id);
            if (order == null)
            {
                return GeneralResult<OrderReadDto>.NotFound("Not Found Order after placed");
            }
            else
            {
                var orderReadDto = order.ToReadDto();
                return GeneralResult<OrderReadDto>.SuccessedResult(orderReadDto, "Order place successfully");
            }
        }

        public async Task<GeneralResult<IEnumerable<OrderReadDto>>> GetAllOrdersByUserIdAsync(string userId)
        {
            var orders = await _unitOfWork.OrderRepository.GetByUserIdWithItemsAsync(userId);
            var ordersReadDto = orders.Select(o => o.ToReadDto()).ToList();
            return GeneralResult<IEnumerable<OrderReadDto>>.SuccessedResult(ordersReadDto);
        }

        public async Task<GeneralResult<OrderReadDto>> GetOrderByIdAsync(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdWithItemsAsync(id);
            if (order == null)
            {
                return GeneralResult<OrderReadDto>.NotFound("There is no order with that id");
            }

            var orderReadDto = order.ToReadDto();
            return GeneralResult<OrderReadDto>.SuccessedResult(orderReadDto);
        }
    }
}
