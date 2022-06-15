using Ardalis.GuardClauses;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FuzzyPaws2.Services
{
    public class SelectListService : ISelectListService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SelectListService(ApplicationDbContext context, 
                                 IWebHostEnvironment webHostEnvironment,
                                 IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<SelectListItem>> GetPetTypes(bool addOptionLabel = true, string optionLabelText = null)
        {
            var petTypes = (await _context.PetType
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
                AddOptionLabel(petTypes, optionLabelText);
            }

            return petTypes;
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

        public async Task<IEnumerable<SelectListItem>> GetPetBreeds(bool addOptionLabel = true, string optionLabelText = null)
        {
            var petBreeds = (await _context.PetBreed
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
                AddOptionLabel(petBreeds, optionLabelText);
            }

            return petBreeds;
        }

        public async Task<IEnumerable<SelectListItem>> GetMyPets(bool addOptionLabel = true, string optionLabelText = null)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var myPets = (await _context.MyPets
                        .Where(x => x.UserId == userId)
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
                AddOptionLabel(myPets, optionLabelText);
            }

            return myPets;

        }
    }
}
