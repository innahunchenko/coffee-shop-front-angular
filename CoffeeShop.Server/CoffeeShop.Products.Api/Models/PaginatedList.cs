namespace CoffeeShop.Products.Api.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; private set; }
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalItems { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            Items = items;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalItems = count;
        }

        //public bool HasPreviousPage => PageIndex > 1;
        //public bool HasNextPage => PageIndex < TotalPages;
    }
}
