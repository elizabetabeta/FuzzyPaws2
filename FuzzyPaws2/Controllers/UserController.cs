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
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(IUserService userService, 
                              IMapper mapper,
                              UserManager<IdentityUser> userManager,
                              RoleManager<IdentityRole> roleManager)
        {
            _userService = userService;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<IActionResult> Admin()
        {
            var admin = await _userService.GetAdminAsync();
            return View(admin);
        }

        public async Task<IActionResult> Vet()
        {
            var vet = await _userService.GetVetAsync();
            return View(vet);
        }

        public async Task<IActionResult> User()
        {
            var user = await _userService.GetUserAsync();
            return View(user);
        }

        //GET
        public IActionResult Delete(string id)
        {
            var userToDelete = _userService.GetById(id);

            var mappedModelUserToDelete = _mapper.Map<UserCreateViewModel>(userToDelete);

            return View(mappedModelUserToDelete);
        }

        //POST
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                await _userManager.DeleteAsync(user);
                TempData["success"] = "User deleted successfully";
                return RedirectToAction("User");             
            }
        }

        //GET ZA VET
        public IActionResult DeleteViewVet(string id)
        {
            var userToDelete = _userService.GetById(id);

            var mappedModelUserToDelete = _mapper.Map<UserCreateViewModel>(userToDelete);

            return View(mappedModelUserToDelete);
        }

        //POST ZA VET
        public async Task<IActionResult> DeleteVet(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"Vet with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                await _userManager.DeleteAsync(user);
                TempData["success"] = "Vet deleted successfully";
                return RedirectToAction("Vet");
            }
        }

        //GET
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = _mapper.Map<UserCreateViewModel>(user);

            return View(model);
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> EditUser(UserCreateViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                await _userManager.UpdateAsync(user);
                TempData["success"] = "User edited successfully";
                return RedirectToAction("User");
            }
        }

        //GET ZA VET
        public IActionResult EditViewVet(string id)
        {
            var userToEdit = _userService.GetById(id);

            var mappedModelUserToEdit = _mapper.Map<UserCreateViewModel>(userToEdit);

            return View(mappedModelUserToEdit);
        }

        //POST ZA VET
        public async Task<IActionResult> EditVet(UserCreateViewModel model)
        {
            var vet = await _userManager.FindByIdAsync(model.Id);
            if (vet == null)
            {
                ViewBag.ErrorMessage = $"Vet with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                vet.UserName = model.UserName;
                vet.Email = model.Email;
                vet.PhoneNumber = model.PhoneNumber;

                await _userManager.UpdateAsync(vet);
                TempData["success"] = "Vet edited successfully";
                return RedirectToAction("Vet");
            }
        }

    }
}
