using E_Commerce.Entities.Repositories;
using E_Commerce.Entities.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace E_Commerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicatonUser> _userManager;
        public UsersController(IUnitOfWork unitOfWork, UserManager<ApplicatonUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }
        public async Task<IActionResult> GetAllUsers()
        {
            var user = await _userManager.GetUserAsync(User);
            var users = _unitOfWork.Users.GetAll(u => u.Id != user.Id);
            return Json(users);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(String id)
        {
            var user = _unitOfWork.Users.GetFirstorDefault(i => i.Id == id);
            if (user != null)
            {
                try
                {
                    _unitOfWork.Users.Remove(user);
                    _unitOfWork.Save();
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> LockUnlock(String id)
        {
            var user = _unitOfWork.Users.GetFirstorDefault(i => i.Id == id);
            if (user != null)
            {
                if (user.LockoutEnd == null || user.LockoutEnd < DateTime.Now)
                {
                    user.LockoutEnd = DateTime.Now.AddDays(1);
                }
                else
                {
                    user.LockoutEnd = DateTime.Now;
                }
                _unitOfWork.Save();
            }
            else
            {

                return NotFound();
            }
            return Ok();
        }


    }
}
