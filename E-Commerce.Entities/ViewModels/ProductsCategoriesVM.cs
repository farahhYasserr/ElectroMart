
using E_Commerce.Entities.Models;
using X.PagedList;


namespace E_Commerce.Entities.ViewModels
{
    public class ProductsCategoriesVM
    {
        public IPagedList<Product> Products { get; set; }
        public IEnumerable<Category> categories { get; set; }

    }
}
