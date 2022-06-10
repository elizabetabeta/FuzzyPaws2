using AutoMapper;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.Pets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyPaws2.Controllers
{
    [Authorize]
    public class PetController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPetService _petService;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PetController(IPetService petService, 
                             IMapper mapper,
                             ApplicationDbContext context,
                             IWebHostEnvironment webHostEnvironment)
        {
            _petService = petService;
            _mapper = mapper;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _petService.GetPetsAsync();
            return View(model);
        }

        public async Task<IActionResult> Available()
        {
            var model = await _petService.GetPetsAsync();
            return View(model);
        }

        public async Task<IActionResult> Unavailable()
        {
            var model = await _petService.GetPetsAsync();
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
        public IActionResult Edit(int id)
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

        //GET
        public IActionResult Buy(int id)
        {
            var pet = _petService.GetById(id);

            var mappedPet = _mapper.Map<PetCreateViewModel>(pet);

            return View(mappedPet);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Buy(int id, PetCreateViewModel model)
        {
            var pet = _petService.GetById(id);

            pet.IsSold = model.IsSold;

            _context.Update(pet);
            _context.SaveChanges();
            TempData["success"] = "Pet bought successfully!";

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

