using FuzzyPaws2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FuzzyPaws2.ViewModels.Pets
{
    public class PetIndexViewModel
    {
        public IEnumerable<Pet> AllPets { get; set; }
        //public int CurrentPageIndex { get; set; }
        //public int PageCount { get; set; }

    }
}
