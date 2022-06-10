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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PetBreedController(ApplicationDbContext context, 
                                  IPetBreedService petBreedService, 
                                  IMapper mapper, 
                                  IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _petBreedService = petBreedService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _petBreedService.GetPetBreedsAsync();
            return View(model);
        }
        //GET
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var breed = _petBreedService.GetById(id);

            //var mappedPetToEdit = _mapper.Map<CreateBreedViewModel>(breedToEdit);

            var model = new CreateBreedViewModel()
            {
                Id = breed.Id,
                Name = breed.Name,
                Description = breed.Description,
                ExistingImage = breed.Image
            };
            return View(model);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CreateBreedViewModel model)
        {
            //_petBreedService.EditAsync(model);
            var breed = _petBreedService.GetById(id);

            breed.Name = model.Name;
            breed.Description = model.Description;

            if (model.Picture != null)
            {
                if (model.ExistingImage != null)
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, FileLocation.FileUploadFolder, model.ExistingImage);
                    System.IO.File.Delete(filePath);
                }

                breed.Image = ProcessUploadedFile(model);
            }

            _context.Update(breed);
            _context.SaveChanges();
            TempData["success"] = "Pet breed edited successfully";

            return RedirectToAction("Details", new
            {
                id = model.Id
            });
        }

        //GET
        [Authorize(Roles = "Admin")]
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

        private string ProcessUploadedFile(CreateBreedViewModel model)
        {
            string uniqueFileName = null;

            if (model.Picture != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, FileLocation.FileUploadFolder);
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Picture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Picture.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
