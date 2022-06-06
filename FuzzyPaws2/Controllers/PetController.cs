using AutoMapper;
using FuzzyPaws2.Core.Model;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.Pets;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyPaws2.Controllers
{
    public class PetController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPetService _petService;
        public PetController(IPetService petService, IMapper mapper)
        {
            _petService = petService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _petService.GetPetsAsync();
            return View(model);
        }


        //GET
        public async Task<IActionResult> Create()         
        {
            var model = await _petService.PrepareCreateViewModelAsync();
            return View(model);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PetCreateViewModel model)
        {
            //Post model
            //Send model to _petService.CreateAsync
            //Map PetCreateViewModel to Pet
            //Add pet to dbContext
            _petService.CreateAsync(model);
            TempData["success"] = "Pet added successfully";

            return RedirectToAction("Index");

        }

        //GET
        public IActionResult Edit(int id)
        {
            var petToEdit = _petService.GetById(id);

            var mappedModelPetToEdit = _mapper.Map<PetCreateViewModel>(petToEdit);

            return View(mappedModelPetToEdit);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PetCreateViewModel model)
        {
            _petService.EditAsync(model);
            TempData["success"] = "Pet edited successfully";

            return RedirectToAction("Details", new
            {
                id = model.Id
        }); 
        }

        //GET
        public IActionResult Delete(int id)
        {
            var petToDelete = _petService.GetById(id);

            var mappedModelPetToDelete = _mapper.Map<PetCreateViewModel>(petToDelete);

            return View(mappedModelPetToDelete);

        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(PetCreateViewModel model)
        {
            _petService.DeleteAsync(model);
            TempData["success"] = "Pet removed successfully";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _petService.GetPetsById(id);
            if (model == null)
                return NotFound();

            return View(model);
        }


    }
}

