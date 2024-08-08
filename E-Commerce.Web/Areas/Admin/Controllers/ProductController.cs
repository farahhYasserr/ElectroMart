using E_Commerce.DataAccess.Data;
using E_Commerce.Entities.Models;
using E_Commerce.Entities.Repositories;
using E_Commerce.Entities.ViewModels;
using E_Commerce.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace E_Commerce.Web.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ProductController : Controller
    {
        IUnitOfWork _unitOfWork;
        IImageService _imageService;
        public ProductController(IImageService imageService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;

        }
        public IActionResult Index()
        {
            var products = _unitOfWork.Products.GetAll();
            return View(products);
        }
        public IActionResult GetAllProducts()
        {
            var Products = _unitOfWork.Products.GetAll(Includeword: "Category");
            return Json(Products);
        }
        public IActionResult Create()
        {
            ProductVM productVM = new ProductVM()
            {
                CategoryList = _unitOfWork.Categories.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                })
            };
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                product.Img = _imageService.UploadImage(file);
                _unitOfWork.Products.Add(product);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            var CategoryList = _unitOfWork.Categories.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });
            return View(new ProductVM() { Product = product, CategoryList = CategoryList });
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var product = _unitOfWork.Products.GetFirstorDefault(c => c.Id == Id, "Category");
            if (product == null)
            {
                return NotFound();
            }
            var CategoryList = _unitOfWork.Categories.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });

            return View(new ProductVM { CategoryList = CategoryList, Product = product });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product, IFormFile? file = null)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    _imageService.DeleteImage(product.Img);
                    product.Img = _imageService.UploadImage(file);
                }
                _unitOfWork.Products.Update(product);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            var CategoryList = _unitOfWork.Categories.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });

            return View(new ProductVM { CategoryList = CategoryList, Product = product });

        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {

            var product = _unitOfWork.Products.GetFirstorDefault(c => c.Id == id, "Category");
            if (product == null)
            {
                return Json(new { success = false, message = "Error " });
            }

            _unitOfWork.Products.Remove(product);
            _unitOfWork.Save();
            return Json(new { success = true });


        }


    }

}
