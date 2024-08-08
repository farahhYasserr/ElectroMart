using E_Commerce.DataAccess.Implementation;
using E_Commerce.Entities.Models;
using E_Commerce.Entities.Repositories;
using E_Commerce.Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace E_Commerce.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicatonUser> _userManger;

        public HomeController(IUnitOfWork unitOfWork, UserManager<ApplicatonUser> userManger)
        {
            _unitOfWork = unitOfWork;
            _userManger = userManger;
        }
        public IActionResult Index(int page = 1, int? categoryId = null)
        {
            int pageSize = 6;
            IPagedList<Product> Products;
            // IEnumerable<Product> random = _unitOfWork.Products.GetAll(u => u.CategoryId == categoryId);


            if (categoryId == null)
            {
                categoryId = 0;
                IEnumerable<Product> allProducts = _unitOfWork.Products.GetAll();
                var shuffledProducts = allProducts.OrderBy(p => Guid.NewGuid()).ToList();
                Products = shuffledProducts.ToPagedList(page, pageSize);

            }
            else
            {
                Products = _unitOfWork.Products.GetAll(u => u.CategoryId == categoryId).ToPagedList(page, pageSize);
            }

            var Categories = _unitOfWork.Categories.GetAll();
            ViewBag.categoryId = categoryId;
            return View(new ProductsCategoriesVM
            {
                Products = Products,
                categories = Categories
            });
        }
        [HttpGet]
        public IActionResult ProductDetails(int Id)
        {
            var Product = _unitOfWork.Products.GetFirstorDefault(p => p.Id == Id);
            return View(new ShoppingCart
            {

                Product = Product,
                Count = 1,
                ProductId = Id

            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddToCart(ShoppingCart ShoppingCart)
        {

            var user = await _userManger.GetUserAsync(User);
            ShoppingCart.User = user;
            ShoppingCart.UserId = user.Id;

            var OldShoppingCart = _unitOfWork.ShoppingCarts.GetFirstorDefault(u =>
            u.ProductId == ShoppingCart.ProductId &&
            u.UserId == ShoppingCart.UserId

            );

            try
            {
                if (OldShoppingCart != null)
                {
                    OldShoppingCart.Count = ShoppingCart.Count;
                    _unitOfWork.ShoppingCarts.Update(OldShoppingCart);
                    _unitOfWork.Save();
                }
                else
                {
                    _unitOfWork.ShoppingCarts.Add(ShoppingCart);
                    _unitOfWork.Save();
                }
                var count = _unitOfWork.ShoppingCarts.GetAll(s => s.
                       UserId == user.Id &&
                      s.OrderId == null).Count();

                HttpContext.Session.SetInt32("ShoppingCount" + user.Id, count);

            }
            catch (Exception)
            {
                return NotFound();
            }
            var x = ShoppingCart.ProductId;
            return RedirectToAction("ProductDetails", new { id = x });
        }
    }
}
