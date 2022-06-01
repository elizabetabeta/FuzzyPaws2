using AutoMapper;
using FuzzyPaws2.Core.Model;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.PetTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FuzzyPaws2.Services
{
    public class PetTypeService : IPetTypeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public PetTypeService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateTypeViewModel> CreateTypeAsync()
        {
            var model = new CreateTypeViewModel();

            return model;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void CreateType(PetType petType)
        {   
           _context.PetType.Add(petType);
           _context.SaveChanges();                
        }
        public PetType GetById(int id)
        {
            return _context.PetType.FirstOrDefault(x => x.Id == id);
        }

        public async Task<PetTypeIndexViewModel> GetPetTypesAsync()
        {
            var model = new PetTypeIndexViewModel()
            {
                AllTypesOfPets = await _context.PetType.ToListAsync()
            };

            return model;
        }

        public async Task<Result> CreateAsync(CreateTypeViewModel model)
        {
            var mappedType = _mapper.Map<PetType>(model);
            _context.Add(mappedType);
            _context.SaveChanges();
            return Result.Success(mappedType);
        }

        public async Task<Result> DeleteAsync(CreateTypeViewModel model)
        {
            var mappedType = _mapper.Map<PetType>(model);
            _context.Remove(mappedType);
            _context.SaveChanges();
            return Result.Success(mappedType);
        }

        public async Task<Result> EditAsync(CreateTypeViewModel model)
        {
            var mappedType = _mapper.Map<PetType>(model);
            _context.Update(mappedType);
            _context.SaveChanges();
            return Result.Success(mappedType);
        }
    }
}
