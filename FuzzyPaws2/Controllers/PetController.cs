using AutoMapper;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.MyPets;
using FuzzyPaws2.ViewModels.Pets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FuzzyPaws2.Controllers
{
    [Authorize]
    public class PetController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPetService _petService;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISelectListService _selectListService;
        public PetController(IPetService petService,
                             IMapper mapper,
                             ApplicationDbContext context,
                             IWebHostEnvironment webHostEnvironment,
                             ISelectListService selectListService)
        {
            _petService = petService;
            _mapper = mapper;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _selectListService = selectListService;
        }

        public async Task<IActionResult> Index(string search, 
                                               string type,
                                               string breed)
        {
            ViewData["CurrentFilter"] = search;
            ViewData["CurrentFilter2"] = type;
            ViewData["CurrentFilter3"] = breed;

            var model = await _petService.GetPetsAsync(search, type, breed);

            return View(model);
        }

        public async Task<IActionResult> Available(string search, string type, string breed)
        {
            ViewData["CurrentFilter"] = search;
            ViewData["CurrentFilter2"] = type;
            ViewData["CurrentFilter3"] = breed;

            var model = await _petService.GetPetsAsync(search, type, breed);
            return View(model);
        }

        public async Task<IActionResult> Unavailable(string search, string type, string breed)
        {
            ViewData["CurrentFilter"] = search;
            ViewData["CurrentFilter2"] = type;
            ViewData["CurrentFilter3"] = breed;

            var model = await _petService.GetPetsAsync(search, type, breed);
            return View(model);
        }

        [Authorize(Roles = "Admin")] 
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
            _petService.CreateAsync(model);
            TempData["success"] = "Pet added successfully";

            return RedirectToAction("Index");

        }

        //GET
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var pet = _petService.GetById(id);

            //var mappedModelPetToEdit = _mapper.Map<PetCreateViewModel>(petToEdit);
            var model = new PetCreateViewModel()
            {
                Id = pet.Id,
                Name = pet.Name,
                Description = pet.Description,
                Price = pet.Price,
                PetTypeId = pet.PetTypeId,
                PetBreedId = pet.PetBreedId,
                ExistingImage = pet.Image
            };

            model.PetTypes = await _selectListService.GetPetTypes(true, "Choose the pet type");
            model.PetBreeds = await _selectListService.GetPetBreeds(true, "Choose the pet type");

            return View(model);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PetCreateViewModel model)
        {
            //_petService.EditAsync(id, model);
            var pet = _petService.GetById(id);

            pet.Name = model.Name;
            pet.Description = model.Description;
            pet.Price = model.Price;
            pet.PetTypeId = model.PetTypeId;
            pet.PetBreedId = model.PetBreedId;

            if (model.Picture != null)
            {
                if (model.ExistingImage != null)
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, FileLocation.FileUploadFolder, model.ExistingImage);
                    System.IO.File.Delete(filePath);
                }

                pet.Image = ProcessUploadedFile(model);
            }

            _context.Update(pet);
            _context.SaveChanges();
            TempData["success"] = "Pet edited successfully";

            return RedirectToAction("Details", new
            {
                id = model.Id
            });
        }

        //GET
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var petToDelete = _petService.GetById(id);

            var mappedModelPetToDelete = _mapper.Map<PetCreateViewModel>(petToDelete);

            mappedModelPetToDelete.PetTypes = await _selectListService.GetPetTypes(true, "Choose the pet type");
            mappedModelPetToDelete.PetBreeds = await _selectListService.GetPetBreeds(true, "Choose the pet type");

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

        //GET
        public async Task<IActionResult> Buy(int id)
        {
            var pet = _petService.GetById(id);

            var mappedPet = _mapper.Map<PetCreateViewModel>(pet);

            mappedPet.PetTypes = await _selectListService.GetPetTypes(true, "Choose the pet type");
            mappedPet.PetBreeds = await _selectListService.GetPetBreeds(true, "Choose the pet type");

            return View(mappedPet);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Buy(int id, PetCreateViewModel model, MyPetCreateViewModel myPet)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var pet = _petService.GetById(id);
            pet.IsSold = model.IsSold;
            _context.Update(pet);

            var newMyPet = new MyPet()
            {
                Name = pet.Name,
                Description = pet.Description,
                PetTypeId = pet.PetTypeId,
                PetBreedId = pet.PetBreedId,
                UserId = userId,
        };

            //Add pet to my pets
            _context.MyPets.Add(newMyPet);
            _context.SaveChanges();
            TempData["success"] = "Pet bought and added to your pets successfully!";

            return RedirectToAction("Details", new
            {
                id = model.Id
            });
        }

        private string ProcessUploadedFile(PetCreateViewModel model)
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

