using AutoMapper;
using FuzzyPaws2.Core.Model;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.MyPets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FuzzyPaws2.Services
{
    public class MyPetService : IMyPetService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISelectListService _selectListService;
        public MyPetService(ApplicationDbContext context,
                            IMapper mapper,
                            UserManager<IdentityUser> userManager,
                            IHttpContextAccessor httpContextAccessor,
                            IWebHostEnvironment webHostEnvironment,
                            ISelectListService selectListService)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _selectListService = selectListService;
        }

        public async Task<MyPetIndexViewModel> ShowMyPetsAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var model = new MyPetIndexViewModel()
            {
                MyPets = await _context.MyPets.Where(x => x.UserId == userId).
                OrderByDescending(x => x.Id).
                ToListAsync()
            };

            return model;
        }

        public async Task<MyPetIndexViewModel> ShowAllMyPetsAsync()
        {
            var model = new MyPetIndexViewModel()
            {
                MyPets = await _context.MyPets.
                OrderByDescending(x => x.Id).
                ToListAsync()
            };

            return model;
        }

        public MyPet GetById(int id)
        {
            return _context.MyPets.FirstOrDefault(x => x.Id == id);
        }

        public async Task<MyPetDetailsViewModel> GetMyPetsById(int petId)
        {
            var pet = _context.MyPets
                .Include(x => x.PetBreed)
                .Include(x => x.PetType)
                .Include(x => x.AspNetUsers)
                .FirstOrDefault(p => p.Id == petId);

            MyPetDetailsViewModel model = _mapper.Map<MyPetDetailsViewModel>(pet);

            return model;
        }

        public async Task<MyPetCreateViewModel> CreateViewModelAsync()
        {
            var model = new MyPetCreateViewModel();
            model.PetTypes = await _selectListService.GetPetTypes(true, "Choose the pet type");
            model.PetBreeds = await _selectListService.GetPetBreeds(true, "Choose the pet breed");

            return model;
        }

        //POST ZA CREATE
        public async Task<Result> CreateAsync(MyPetCreateViewModel model)
        {
            //var mappedPet = _mapper.Map<MyPet>(model);

            string uniqueFileName = ProcessUploadedFile(model);

            MyPet pet = new MyPet
            {
                Name = model.Name,
                Description = model.Description,
                PetTypeId = model.PetTypeId,
                PetBreedId = model.PetBreedId,
                UserId = model.UserId,
                Image = uniqueFileName
            };

            _context.MyPets.Add(pet);
            _context.SaveChanges();

            return Result.Success(pet);
        }

        public async Task<Result> DeleteAsync(MyPetCreateViewModel model)
        {
            var mappedPet = _mapper.Map<MyPet>(model);

            _context.MyPets.Remove(mappedPet);
            _context.SaveChanges();

            return Result.Success(mappedPet);
        }

        public async Task<Result> EditAsync(MyPetCreateViewModel model)
        {
            var mappedPet = _mapper.Map<MyPet>(model);

            _context.MyPets.Update(mappedPet);
            _context.SaveChanges();

            return Result.Success(mappedPet);
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
