using FuzzyPaws2.Core.Model;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.PetBreeds;

namespace FuzzyPaws2.Interfaces
{
    public interface IPetBreedService
    {
        Task<PetBreedIndexViewModel> GetPetBreedsAsync();
        Task<CreateBreedViewModel> PrepareCreateBreedModelAsync();
        Task<BreedDetailsViewModel> GetBreedsByIdForDetails(int breedId);

        Task<Result> CreateAsync(CreateBreedViewModel model);
        Task<Result> DeleteAsync(CreateBreedViewModel model);
        Task<Result> EditAsync(CreateBreedViewModel model);
        PetBreed GetById(int id);
    }
}
