

namespace ECommerce.Common
{
    public class ProductFilter : BaseFilter
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool inStockOnly { get; set; }
    }
}
