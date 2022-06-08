using AutoMapper;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.ViewModels.MyPets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyPaws2.Controllers
{
    [Authorize]
    public class MyPetController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMyPetService _myPetService;
        public MyPetController(IMyPetService myPetService, IMapper mapper)
        {
            _myPetService = myPetService;
            _mapper = mapper;
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
    }
}
