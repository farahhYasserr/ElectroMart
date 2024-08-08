using E_Commerce.DataAccess.Data;
using E_Commerce.Entities.Models;
using E_Commerce.Entities.Repositories;
using E_Commerce.Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace E_Commerce.Web.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult GetAllCategories()
        {
            var categories = _unitOfWork.Categories.GetAll();
            return Json(categories);
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {

                _unitOfWork.Categories.Add(category);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Edit(int Id)
        {
            var category = _unitOfWork.Categories.GetFirstorDefault(c => c.Id == Id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Categories.Update(category);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(category);
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {

            var category = _unitOfWork.Categories.GetFirstorDefault(c => c.Id == id);
            if (category == null)
            {
                return Json(new { success = false, message = "Error " });
            }

            _unitOfWork.Categories.Remove(category);
            _unitOfWork.Save();
            return Json(new { success = true });


        }
    }
}
