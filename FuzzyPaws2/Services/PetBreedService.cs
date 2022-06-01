using AutoMapper;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.PetBreeds;
using Microsoft.EntityFrameworkCore;

namespace FuzzyPaws2.Services
{
    public class PetBreedService : IPetBreedService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public PetBreedService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PetBreedIndexViewModel> GetPetBreedsAsync()
        {
            var model = new PetBreedIndexViewModel()
            {
                AllBreedsOfPets = await _context.PetBreed.ToListAsync()
            };

            return model;
        }

        public PetBreed GetById(int id)
        {
            return _context.PetBreed.FirstOrDefault(x => x.Id == id);
        }

        public async Task<CreateBreedViewModel> PrepareCreateBreedModelAsync()
        {
            var model = new CreateBreedViewModel();

            return model;
        }
    }
}
