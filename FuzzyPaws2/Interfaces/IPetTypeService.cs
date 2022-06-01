using FuzzyPaws2.Core.Model;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.PetTypes;

namespace FuzzyPaws2.Interfaces
{
    public interface IPetTypeService
    {
        Task<PetTypeIndexViewModel> GetPetTypesAsync();
        Task<CreateTypeViewModel> CreateTypeAsync();
        Task<Result> CreateAsync(CreateTypeViewModel model);
        Task<Result> DeleteAsync(CreateTypeViewModel model);
        Task<Result> EditAsync(CreateTypeViewModel model);
        PetType GetById(int id);
    }
}
