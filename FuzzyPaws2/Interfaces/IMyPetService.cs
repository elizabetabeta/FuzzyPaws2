using FuzzyPaws2.Core.Model;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.MyPets;

namespace FuzzyPaws2.Interfaces
{
    public interface IMyPetService
    {
        Task<MyPetIndexViewModel> ShowMyPetsAsync();
        Task<MyPetDetailsViewModel> GetMyPetsById(int petId);
        MyPet GetById(int id);
        Task<MyPetCreateViewModel> CreateViewModelAsync();
        Task<Result> CreateAsync(MyPetCreateViewModel model);
        Task<Result> DeleteAsync(MyPetCreateViewModel model);
        Task<Result> EditAsync(MyPetCreateViewModel model);

    }
}
