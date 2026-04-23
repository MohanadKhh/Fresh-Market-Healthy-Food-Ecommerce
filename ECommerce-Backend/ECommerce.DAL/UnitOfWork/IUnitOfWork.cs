using ECommerce.DAL;

public interface IUnitOfWork
{
    ICartRepository CartRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IOrderRepository OrderRepository { get; }
    IProductRepository ProductRepository { get; }

    Task SaveAsync();
}