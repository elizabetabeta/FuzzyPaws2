using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.PetBreeds;

namespace FuzzyPaws2.Interfaces
{
    public interface IPetBreedService
    {
        Task<PetBreedIndexViewModel> GetPetBreedsAsync();
        Task<CreateBreedViewModel> PrepareCreateBreedModelAsync();
        PetBreed GetById(int id);
    }
}
