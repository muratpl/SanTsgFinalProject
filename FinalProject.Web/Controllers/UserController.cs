using FinalProject.Application.Services.Interfaces;
using FinalProject.Domain.Users;
using Microsoft.AspNetCore.Mvc;


namespace FinalProject.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult List()
        {
            var users = _userService.List();
            return View(users);
        }
        
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _userService.Create(user);
                TempData["Create"] = "User created successfully...";
                return RedirectToAction("List");
            }
            return View(user);
        }

        public IActionResult Delete(int id)
        {

            _userService.Delete(id);
            TempData["Delete"] = "User deleted successfully...";
            return RedirectToAction("List");
        }

        public IActionResult ChangeStatus(int id)
        {
            _userService.ChangeStatus(id);
            return RedirectToAction("List");
        }
        
        

    }
}
