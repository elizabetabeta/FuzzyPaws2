using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.PetTypes;

namespace FuzzyPaws2.Interfaces
{
    public interface IPetTypeService
    {
        Task<PetTypeIndexViewModel> GetPetTypesAsync();
        Task<CreateTypeViewModel> CreateTypeAsync();
        PetType GetById(int id);
    }
}
