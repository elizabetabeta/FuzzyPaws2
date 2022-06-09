using AutoMapper;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.ViewModels.MyPets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyPaws2.Controllers
{
    [Authorize]
    public class MyPetController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMyPetService _myPetService;
        private readonly UserManager<IdentityUser> _userManager;
        public MyPetController(IMyPetService myPetService, 
                               IMapper mapper,
                               UserManager<IdentityUser> userManager)
        {
            _myPetService = myPetService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _myPetService.ShowMyPetsAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _myPetService.GetMyPetsById(id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        //GET
        public async Task<IActionResult> Create()
        {
            var model = await _myPetService.CreateViewModelAsync();
            return View(model);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MyPetCreateViewModel model)
        {
            _myPetService.CreateAsync(model);
            TempData["success"] = "Your pet added successfully!";

            return RedirectToAction("Index");

        }

        //GET
        public IActionResult Edit(int id)
        {
            var petToEdit = _myPetService.GetById(id);

            var mappedModel = _mapper.Map<MyPetCreateViewModel>(petToEdit);

            return View(mappedModel);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MyPetCreateViewModel model)
        {
            _myPetService.EditAsync(model);
            TempData["success"] = "Your pet edited successfully";

            return RedirectToAction("Details", new
            {
                id = model.Id
            });
        }

        //GET
        public IActionResult Delete(int id)
        {
            var petToDelete = _myPetService.GetById(id);

            var mappedModelPetToDelete = _mapper.Map<MyPetCreateViewModel>(petToDelete);

            return View(mappedModelPetToDelete);

        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(MyPetCreateViewModel model)
        {
            _myPetService.DeleteAsync(model);
            TempData["success"] = "Your pet removed successfully";

            return RedirectToAction("Index");
        }
    }
}
