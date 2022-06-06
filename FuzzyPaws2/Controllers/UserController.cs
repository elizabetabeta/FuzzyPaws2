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

        //GET
        public IActionResult Edit(string id)
        {
            var userToEdit = _userService.GetById(id);

            var mappedModelUserToEdit = _mapper.Map<UserCreateViewModel>(userToEdit);

            return View(mappedModelUserToEdit);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserCreateViewModel model)
        {
            _userService.EditAsync(model);
            TempData["success"] = "User info edited successfully";

            return RedirectToAction("Index");        
        }
    }
}
