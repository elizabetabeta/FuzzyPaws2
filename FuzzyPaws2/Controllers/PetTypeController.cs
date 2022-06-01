using AutoMapper;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.PetTypes;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyPaws2.Controllers
{
    public class PetTypeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPetTypeService _petTypeService;
        private readonly IMapper _mapper;
        public PetTypeController(ApplicationDbContext context, IPetTypeService petTypeService, IMapper mapper)
        {
            _context = context;
            _petTypeService = petTypeService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _petTypeService.GetPetTypesAsync();
            return View(model);
        }

        //GET
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
        public IActionResult Edit(int id)
        {
            var typeToEdit = _petTypeService.GetById(id);
            var mappedTypeToEdit = _mapper.Map<CreateTypeViewModel>(typeToEdit);

            return View(mappedTypeToEdit);

        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CreateTypeViewModel model)
        {
            _petTypeService.EditAsync(model);
            TempData["success"] = "Pet type edited successfully";

            return RedirectToAction("Index");
        }

        //GET
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
    }
}
