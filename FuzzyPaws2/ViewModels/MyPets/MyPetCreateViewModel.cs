﻿using FuzzyPaws2.ViewModels.Images;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FuzzyPaws2.ViewModels.MyPets
{
    public class MyPetCreateViewModel : EditImageViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PetBreedId { get; set; }
        public int PetTypeId { get; set; }
        public string UserId { get; set; }
        public IEnumerable<SelectListItem> PetTypes { get; set; }
        public IEnumerable<SelectListItem> PetBreeds { get; set; }
    }
}
