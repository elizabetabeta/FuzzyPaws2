using Ardalis.GuardClauses;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FuzzyPaws2.Services
{
    public class SelectListService : ISelectListService
    {
        private readonly ApplicationDbContext _context;
        public SelectListService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetPetTypes(bool addOptionLabel = true, string optionLabelText = null)
        {
            var vehicleBrands = (await _context.PetType
                        .OrderBy(s => s.Name)
                        .ToListAsync())
              .Select(c => new SelectListItem
              {
                  Text = c.Name,
                  Value = c.Id.ToString()
              })
              .ToList();

            if (addOptionLabel)
            {
                AddOptionLabel(vehicleBrands, optionLabelText);
            }

            return vehicleBrands;
        }

        IEnumerable<SelectListItem> ISelectListService.GetEnumNameList<TEnum>(bool addOptionLabel, string optionLabelText)
        {
            throw new NotImplementedException();
        }

        #region Internals

        /// <summary>
        /// Appends optional label to items list
        /// </summary>
        /// <param name="items"></param>
        /// <param name="optionLabelText"></param>
        /// <param name="optionLabelValue">Used in cases when we want to map null instead of zero</param>
        private void AddOptionLabel(IList<SelectListItem> items, string optionLabelText = null)
        {
            Guard.Against.Null(items, nameof(items));

            optionLabelText ??= "Izaberi opciju";

            items.Insert(0, new SelectListItem(optionLabelText, string.Empty));
        }

        #endregion Internals
    }
}
