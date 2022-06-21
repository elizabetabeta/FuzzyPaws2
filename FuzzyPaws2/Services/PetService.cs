using AutoMapper;
using FuzzyPaws2.Core.Model;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.Pets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FuzzyPaws2.Services
{
    public class PetService : IPetService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISelectListService _selectListService;

        public PetService(ApplicationDbContext context,
                          IMapper mapper,
                          IWebHostEnvironment webHostEnvironment, 
                          ISelectListService selectListService)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _selectListService = selectListService;
        }

        //GET ZA CREATE, EDIT I DELETE VIEW 
        public async Task<PetCreateViewModel> PrepareCreateViewModelAsync()
        {
            var model = new PetCreateViewModel();
            model.PetTypes = await _selectListService.GetPetTypes(true, "Choose the pet type");
            model.PetBreeds = await _selectListService.GetPetBreeds(true, "Choose the pet breed");

            return model;
        }

        //GET ZA VIEW SVIH PET
        public async Task<PetIndexViewModel> GetPetsAsync(string? search, 
                                                          string? type, 
                                                          string? breed
                                                          /*int currentPage , [FromQuery] PetParameters petParameters*/)
        {
            //int maxRows = 10;

            if (!String.IsNullOrEmpty(search))
            {
                var searchModel = new PetIndexViewModel()
                {
                    AllPets = await _context.Pets.Where(x => x.Name.Contains(search))
                    .OrderByDescending(x => x.Id)
                    .ToListAsync()
                };

                return searchModel;

            }

            else if (!String.IsNullOrEmpty(type))
            {
                var typeModel = new PetIndexViewModel()
                {
                    AllPets = await _context.Pets.Where(x => x.PetType.Name.Contains(type))
                    .OrderByDescending(x => x.Id)
                    .ToListAsync()
                };

                return typeModel;

            }
            else if (!String.IsNullOrEmpty(breed))
            {
                var breedModel = new PetIndexViewModel()
                {
                    AllPets = await _context.Pets.Where(x => x.PetBreed.Name.Contains(breed))
                    .OrderByDescending(x => x.Id)
                    .ToListAsync()
                };

                return breedModel;

            }

            var model = new PetIndexViewModel()
            {
                AllPets = await _context.Pets
                .OrderByDescending(x => x.Id)
                //.Skip((currentPage - 1) * maxRows)
                //.Take(maxRows)
                .ToListAsync()
            };

            //double pageCount = (double)((decimal)_context.Pets.Count() / Convert.ToDecimal(maxRows));
            //model.PageCount = (int)Math.Ceiling(pageCount);

            //model.CurrentPageIndex = currentPage;

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
