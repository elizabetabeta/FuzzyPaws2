using Microsoft.AspNetCore.Mvc.Rendering;

namespace FuzzyPaws2.Interfaces
{
    public interface ISelectListService
    {
        Task<IEnumerable<SelectListItem>> GetPetTypes(bool addOptionLabel = true, string optionLabelText = null);
        IEnumerable<SelectListItem> GetEnumNameList<TEnum>(bool addOptionLabel = true, string optionLabelText = null) where TEnum : Enum;

    }
}
