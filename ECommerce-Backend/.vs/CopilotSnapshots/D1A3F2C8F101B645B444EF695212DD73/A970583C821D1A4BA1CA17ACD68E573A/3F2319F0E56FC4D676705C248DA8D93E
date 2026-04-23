using ECommerce.DAL;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.BLL
{
    public class CategoryReadDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public required string Name { get; set; }

        public int ProductCounts { get; set; }
        public IEnumerable<ProductReadDto>? products { get; set; }
    }
}
