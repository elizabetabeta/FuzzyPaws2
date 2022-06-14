﻿using FuzzyPaws2.ViewModels.Images;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FuzzyPaws2.ViewModels.Pets
{
    public class PetCreateViewModel : EditImageViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool IsSold { get; set; }
        public int PetBreedId { get; set; }
        public int PetTypeId { get; set; }
        public IEnumerable<SelectListItem> PetTypes { get; set; }
    }
}

