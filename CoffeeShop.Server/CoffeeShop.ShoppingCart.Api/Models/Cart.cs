using CoffeeShop.DataAccess.Common;

namespace CoffeeShop.ShoppingCart.Api.Models
{
    public class Cart : BaseEntity
    {
        public int UserId { get; set; }
        public List<ProductSelection> Selections { get; set; }
        public int ItemCount { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
