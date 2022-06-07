using AutoMapper;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyPaws2.Controllers
{
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(IUserService userService, 
                              IMapper mapper, 
                              UserManager<IdentityUser> userManager)
        {
            _userService = userService;
            _mapper = mapper;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var model = await _userService.GetAdminAsync();
            return View(model);
        }

        public async Task<IActionResult> Vets()
        {
            var model = await _userService.GetVetAsync();
            return View(model);
        }

        public async Task<IActionResult> Users()
        {
            var model = await _userService.GetUserAsync();
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

            return RedirectToAction("Users");
        }

        public IActionResult DeleteVet(string id)
        {
            var userToDelete = _userService.GetById(id);

            var mappedModelUserToDelete = _mapper.Map<UserCreateViewModel>(userToDelete);

            return View(mappedModelUserToDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteVet(UserCreateViewModel model)
        {
            _userService.DeleteAsync(model);
            TempData["success"] = "Vet deleted successfully";

            return RedirectToAction("Vets");
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

            return RedirectToAction("Users");        
        }

        public IActionResult EditVet(string id)
        {
            var userToEdit = _userService.GetById(id);

            var mappedModelUserToEdit = _mapper.Map<UserCreateViewModel>(userToEdit);

            return View(mappedModelUserToEdit);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditVet(UserCreateViewModel model)
        {
            _userService.EditAsync(model);
            TempData["success"] = "Vet info edited successfully";

            return RedirectToAction("Vets");
        }

    }
}
