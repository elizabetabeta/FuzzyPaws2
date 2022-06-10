using AutoMapper;
using FuzzyPaws2.Core.Model;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PetBreedService(ApplicationDbContext context, 
                               IMapper mapper,
                               IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
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

        public async Task<Result> CreateAsync(CreateBreedViewModel model)
        {
            string uniqueFileName = ProcessUploadedFile(model);

            PetBreed pet = new PetBreed
            {
                Name = model.Name,
                Description = model.Description,
                Image = uniqueFileName
            };

            _context.PetBreed.Add(pet);
            _context.SaveChanges();

            return Result.Success(pet);
        }

        public async Task<Result> DeleteAsync(CreateBreedViewModel model)
        {
            var mappedBreed = _mapper.Map<PetBreed>(model);

            _context.PetBreed.Remove(mappedBreed);
            _context.SaveChanges();

            return Result.Success(mappedBreed);
        }

        public async Task<Result> EditAsync(CreateBreedViewModel model)
        {
            var mappedBreed = _mapper.Map<PetBreed>(model);

            _context.PetBreed.Update(mappedBreed);
            _context.SaveChanges();

            return Result.Success(mappedBreed);
        }

        public async Task<BreedDetailsViewModel> GetBreedsByIdForDetails(int breedId)
        {
            var breed = _context.PetBreed.FirstOrDefault(p => p.Id == breedId);

            BreedDetailsViewModel breedDetailsViewModel = _mapper.Map<BreedDetailsViewModel>(breed);

            return breedDetailsViewModel;
        }

        private string ProcessUploadedFile(CreateBreedViewModel model)
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
