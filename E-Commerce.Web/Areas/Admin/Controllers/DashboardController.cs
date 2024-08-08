using E_Commerce.Entities.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace E_Commerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {

        private IUnitOfWork _unitOfWork;
        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ViewBag.Orders = _unitOfWork.Orders.GetAll().Count();
            ViewBag.Users = _unitOfWork.Users.GetAll().Count();
            ViewBag.Products = _unitOfWork.Products.GetAll().Count();
            ViewBag.Categories = _unitOfWork.Categories.GetAll().Count();

            return View();
        }
    }
}
