

namespace ECommerce.Common.Pagination
{
    public class PaginationResult<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public PaginationMetadata Metadata { get; set; } = new();
    }
}
