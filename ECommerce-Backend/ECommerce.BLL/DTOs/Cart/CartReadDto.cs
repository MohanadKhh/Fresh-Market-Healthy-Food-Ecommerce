using ECommerce.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.BLL
{
    public class CartReadDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public ICollection<CartItemReadDto> CartItems { get; set; } = new HashSet<CartItemReadDto>();
    }
}
