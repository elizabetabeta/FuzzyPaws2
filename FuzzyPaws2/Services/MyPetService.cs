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

        public MyPetService(ApplicationDbContext context, 
                            IMapper mapper,
                            UserManager<IdentityUser> userManager,
                            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
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

            return model;
        }

        //POST ZA CREATE
        public async Task<Result> CreateAsync(MyPetCreateViewModel model)
        {
            var mappedPet = _mapper.Map<MyPet>(model);

            _context.MyPets.Add(mappedPet);
            _context.SaveChanges();

            return Result.Success(mappedPet);
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
    }
}
