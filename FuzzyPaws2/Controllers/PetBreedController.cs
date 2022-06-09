using AutoMapper;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.PetBreeds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyPaws2.Controllers
{
    [Authorize]
    public class PetBreedController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPetBreedService _petBreedService;
        private readonly IMapper _mapper;
        public PetBreedController(ApplicationDbContext context, IPetBreedService petBreedService, IMapper mapper)
        {
            _context = context;
            _petBreedService = petBreedService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _petBreedService.GetPetBreedsAsync();
            return View(model);
        }
        //GET
        public async Task<IActionResult> Create()
        {
            var model = await _petBreedService.PrepareCreateBreedModelAsync();
            return View(model);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateBreedViewModel model)
        {
            _petBreedService.CreateAsync(model);
            TempData["success"] = "New breed added successfully";

            return RedirectToAction("Index");
        }

        //GET
        public IActionResult Edit(int id)
        {
            var breedToEdit = _petBreedService.GetById(id);

            var mappedPetToEdit = _mapper.Map<CreateBreedViewModel>(breedToEdit);

            return View(mappedPetToEdit);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CreateBreedViewModel model)
        {
            _petBreedService.EditAsync(model);
            TempData["success"] = "Breed edited successfully";

            return RedirectToAction("Details", new
            {
                id = model.Id
            }); 
        }

        //GET
        public IActionResult Delete(int id)
        {
            var breedToDelete = _petBreedService.GetById(id);

            var mappedPetToDelete = _mapper.Map<CreateBreedViewModel>(breedToDelete);

            return View(mappedPetToDelete);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(CreateBreedViewModel model)
        {
            _petBreedService.DeleteAsync(model);
            TempData["success"] = "Breed removed successfully";

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _petBreedService.GetBreedsByIdForDetails(id);
            if (model == null)
                return NotFound();

            return View(model);
        }
    }
}
