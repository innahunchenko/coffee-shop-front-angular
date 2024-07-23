namespace CoffeeShop.ShoppingCart.Api.Models
{
    public class Cart : BaseEntity
    {
        public int UserId { get; set; }
        public List<ProductSelection> Selections { get; set; }
        public int ItemCount
        {
            get { return Selections.Sum(s => s.Quantity); }
        }
        public decimal TotalPrice { get; set; }
    }
}
