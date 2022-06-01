using AutoMapper;
using FuzzyPaws2.Core.Model;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.Pets;
using Microsoft.EntityFrameworkCore;

namespace FuzzyPaws2.Services
{
    public class PetService : IPetService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PetService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //GET ZA CREATE, EDIT I DELETE VIEW 
        public async Task<PetCreateViewModel> PrepareCreateViewModelAsync()
        {
            var model = new PetCreateViewModel();

            return model;
        }

        //GET ZA VIEW SVIH PET
        public async Task<PetIndexViewModel> GetPetsAsync()
        {
            var model = new PetIndexViewModel()
            {
                AllPets = await _context.Pets.ToListAsync()
            };

            return model;
        }

        //GET ZA PRIKAZ DETALJA POJEDINACNOG PET
        public async Task<PetDetailsViewModel> GetPetsById(int petId)
        {
            var pet = _context.Pets
                .Include(x => x.PetBreed)
                .Include(x => x.PetType)
                .FirstOrDefault(p => p.Id == petId);

            PetDetailsViewModel petDetailsViewModel = _mapper.Map<PetDetailsViewModel>(pet);

            return petDetailsViewModel;
        }

        //GET PET SA ID
        public Pet GetById(int id)
        {
            return _context.Pets.FirstOrDefault(x => x.Id == id);
        }

        //POST ZA CREATE PET
        public async Task<Result> CreateAsync(PetCreateViewModel model)
        {
            var mappedPet = _mapper.Map<Pet>(model);

            _context.Pets.Add(mappedPet);
            _context.SaveChanges();
            
            return Result.Success(mappedPet);
        }

        //POST ZA DELETE PET
        public async Task<Result> DeleteAsync(PetCreateViewModel model)
        {
            var mappedPet = _mapper.Map<Pet>(model);

            _context.Pets.Remove(mappedPet);
            _context.SaveChanges();

            return Result.Success(mappedPet);
        }

        //POST ZA EDIT PET
        public async Task<Result> EditAsync(PetCreateViewModel model)
        {
            var mappedPet = _mapper.Map<Pet>(model);

            _context.Pets.Update(mappedPet);
            _context.SaveChanges();

            return Result.Success(mappedPet);
        }


    }  
    
}
