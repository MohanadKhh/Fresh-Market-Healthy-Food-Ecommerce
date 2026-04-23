
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.DAL
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;

        [NotMapped]
        public decimal TotalPrice { get; private set; }
        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
        public void CalculateTotalPrice()
        {
            TotalPrice = CartItems.Sum(ci => ci.Product.Price * ci.Quantity);
        }
    }
}
