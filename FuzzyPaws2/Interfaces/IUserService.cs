using FuzzyPaws2.Core.Model;
using FuzzyPaws2.ViewModels.Users;
using Microsoft.AspNetCore.Identity;

namespace FuzzyPaws2.Interfaces
{
    public interface IUserService
    {
        Task<UserIndexViewModel> GetAdminAsync();
        Task<UserIndexViewModel> GetVetAsync();
        Task<UserIndexViewModel> GetUserAsync(string search);
        Task<UserCreateViewModel> PrepareCreateViewModelAsync();
        IdentityUser GetById(string id);

    }
}
