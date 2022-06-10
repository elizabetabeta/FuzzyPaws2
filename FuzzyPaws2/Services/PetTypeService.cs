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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PetTypeService(ApplicationDbContext context, 
                              IMapper mapper,
                              IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
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
            //var mappedType = _mapper.Map<PetType>(model);

            string uniqueFileName = ProcessUploadedFile(model);

            PetType pet = new PetType
            {
                Name = model.Name,
                Description = model.Description,
                Image = uniqueFileName
            };

            _context.PetType.Add(pet);
            _context.SaveChanges();

            return Result.Success(pet);
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

        public async Task<TypeDetailsViewModel> GetTypesByIdForDetails(int typeId)
        {
            var type = _context.PetType.FirstOrDefault(p => p.Id == typeId);

            TypeDetailsViewModel typeDetailsViewModel = _mapper.Map<TypeDetailsViewModel>(type);

            return typeDetailsViewModel;
        }

        private string ProcessUploadedFile(CreateTypeViewModel model)
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
