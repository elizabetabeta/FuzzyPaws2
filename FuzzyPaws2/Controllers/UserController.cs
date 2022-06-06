using AutoMapper;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyPaws2.Controllers
{
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index()
        {
            var model = await _userService.GetUsersAsync();
            return View(model);
        }


        //GET
        public IActionResult Delete(string id)
        {
            var userToDelete = _userService.GetById(id);

            var mappedModelUserToDelete = _mapper.Map<UserCreateViewModel>(userToDelete);

            return View(mappedModelUserToDelete);

        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(UserCreateViewModel model)
        {
            _userService.DeleteAsync(model);
            TempData["success"] = "User deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
