using AutoMapper;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.PetBreeds;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyPaws2.Controllers
{
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
        public IActionResult Create(PetBreed breed)
        {
            if (ModelState.IsValid)
            {
                _context.PetBreed.Add(breed);
                _context.SaveChanges();
                TempData["success"] = "Pet breed added successfully";
                return RedirectToAction("Index");
            }
            return View(breed);
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
        public IActionResult Edit(PetBreed breed)
        {
            if (ModelState.IsValid)
            {
                _context.PetBreed.Update(breed);
                _context.SaveChanges();
                TempData["success"] = "Pet type edited successfully";
                return RedirectToAction("Index");
            }
            return View(breed);
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
        public IActionResult DeletePetBreedFromDb(int id)
        {
            var breed = _context.PetBreed.Find(id);
            if (breed == null)
            {
                return NotFound();
            }

            _context.PetBreed.Remove(breed);
            _context.SaveChanges();
            TempData["success"] = "Pet breed removed successfully";
            return RedirectToAction("Index");

        }
    }
}
