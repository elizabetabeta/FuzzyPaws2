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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PetService(ApplicationDbContext context, 
                          IMapper mapper,
                          IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
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
                AllPets = await _context.Pets.OrderByDescending(x => x.Id).ToListAsync()
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
            string uniqueFileName = ProcessUploadedFile(model);

            Pet pet = new Pet
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                PetTypeId = model.PetTypeId,
                PetBreedId = model.PetBreedId,
                Image = uniqueFileName
            };

            //var mappedPet = _mapper.Map<Pet>(model);

            _context.Pets.Add(pet);
            _context.SaveChanges();
            
            return Result.Success(pet);
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
        public async Task<Result> EditAsync(int id, PetCreateViewModel model)
        {
            //var mappedPet = _mapper.Map<Pet>(model);
            var pet = await _context.Pets.FindAsync(model.Id);

            pet.Name = model.Name;
            pet.Description = model.Description;
            pet.Price = model.Price;
            pet.PetTypeId = model.PetTypeId;
            pet.PetBreedId = model.PetBreedId;

            if (model.Picture != null)
            {
                if (model.ExistingImage != null)
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, FileLocation.FileUploadFolder, model.ExistingImage);
                    System.IO.File.Delete(filePath);
                }

                pet.Image = ProcessUploadedFile(model);
            }

            _context.Update(pet);
            await _context.SaveChangesAsync();

            return Result.Success(pet);
        }

        private string ProcessUploadedFile(PetCreateViewModel model)
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
