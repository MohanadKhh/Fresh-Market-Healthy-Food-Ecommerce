using ECommerce.DAL;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IProductRepository ProductRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public ICartRepository CartRepository { get; }
    public IOrderRepository OrderRepository { get; }

    public UnitOfWork(AppDbContext context,
                      IProductRepository products,
                      ICategoryRepository categories,
                      ICartRepository carts,
                      IOrderRepository orders)
    {
        _context = context;
        ProductRepository = products;
        CategoryRepository = categories;
        CartRepository = carts;
        OrderRepository = orders;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}