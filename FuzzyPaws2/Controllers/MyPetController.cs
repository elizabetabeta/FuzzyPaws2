using AutoMapper;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.Models;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;

        public MyPetController(IMyPetService myPetService, 
                               IMapper mapper,
                               UserManager<IdentityUser> userManager,
                               ApplicationDbContext context,
                               IWebHostEnvironment webHostEnvironment)
        {
            _myPetService = myPetService;
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
            var pet = _myPetService.GetById(id);

            var model = new MyPetCreateViewModel()
            {
                Id = pet.Id,
                Name = pet.Name,
                Description = pet.Description,
                PetTypeId = pet.PetTypeId,
                PetBreedId = pet.PetBreedId,
                ExistingImage = pet.Image
            };
            return View(model);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, MyPetCreateViewModel model)
        {
            //_myPetService.EditAsync(model);
            var pet = _myPetService.GetById(id);

            pet.Name = model.Name;
            pet.Description = model.Description;
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

        private string ProcessUploadedFile(MyPetCreateViewModel model)
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
