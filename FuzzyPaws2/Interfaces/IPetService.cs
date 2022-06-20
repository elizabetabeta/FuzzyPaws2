using FuzzyPaws2.Core.Model;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.Pets;

namespace FuzzyPaws2.Interfaces
{
    public interface IPetService
    {
        Task<PetIndexViewModel> GetPetsAsync(string search, string type, string breed/*, PetParameters petParameters*/);
        Task<PetCreateViewModel> PrepareCreateViewModelAsync();
        Task<PetDetailsViewModel> GetPetsById(int petId);
        Task<Result> CreateAsync(PetCreateViewModel model);
        Task<Result> DeleteAsync(PetCreateViewModel model);
        Task<Result> EditAsync(int id, PetCreateViewModel model);
        Pet GetById(int id);
    }
}
