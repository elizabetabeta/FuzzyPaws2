using AutoMapper;
using FuzzyPaws2.Core.Model;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.MyPets;
using Microsoft.EntityFrameworkCore;

namespace FuzzyPaws2.Services
{
    public class MyPetService : IMyPetService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MyPetService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MyPetIndexViewModel> ShowMyPetsAsync()
        {
            var model = new MyPetIndexViewModel()
            {
                MyPets = await _context.MyPets.ToListAsync()
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
    }
}
