using AutoMapper;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.PetTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyPaws2.Controllers
{
    [Authorize]
    public class PetTypeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPetTypeService _petTypeService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PetTypeController(ApplicationDbContext context, 
                                 IPetTypeService petTypeService,
                                 IMapper mapper,
                                 IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _petTypeService = petTypeService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _petTypeService.GetPetTypesAsync();
            return View(model);
        }

        //GET
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var model = await _petTypeService.CreateTypeAsync();
            return View(model);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateTypeViewModel model)
        {
            _petTypeService.CreateAsync(model);
            TempData["success"] = "Pet type added successfully";

            return RedirectToAction("Index");
            
        }

        //GET
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var type = _petTypeService.GetById(id);
            //var mappedTypeToEdit = _mapper.Map<CreateTypeViewModel>(typeToEdit);

            var model = new CreateTypeViewModel()
            {
                Id = type.Id,
                Name = type.Name,
                Description = type.Description,
                ExistingImage = type.Image
            };
            return View(model);

        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CreateTypeViewModel model)
        {
            var type = _petTypeService.GetById(id);

            type.Name = model.Name;
            type.Description = model.Description;

            if (model.Picture != null)
            {
                if (model.ExistingImage != null)
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, FileLocation.FileUploadFolder, model.ExistingImage);
                    System.IO.File.Delete(filePath);
                }

                type.Image = ProcessUploadedFile(model);
            }

            _context.Update(type);
            _context.SaveChanges();
            TempData["success"] = "Pet type edited successfully";

            return RedirectToAction("Details", new
            {
                id = model.Id
            });
        }

        //GET
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var typeToDelete = _petTypeService.GetById(id);

            var mappedTypeToDelete = _mapper.Map<CreateTypeViewModel>(typeToDelete);

            return View(mappedTypeToDelete);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(CreateTypeViewModel model)
        {
            _petTypeService.DeleteAsync(model);
            TempData["success"] = "Pet type removed successfully";

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _petTypeService.GetTypesByIdForDetails(id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        private string ProcessUploadedFile(CreateTypeViewModel model)
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
