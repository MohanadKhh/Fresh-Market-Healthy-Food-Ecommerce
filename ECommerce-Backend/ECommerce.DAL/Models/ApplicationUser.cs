
using Microsoft.AspNetCore.Identity;

namespace ECommerce.DAL
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public virtual Cart? Cart { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
