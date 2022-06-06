using FuzzyPaws2.Core.Model;
using FuzzyPaws2.ViewModels.Users;
using Microsoft.AspNetCore.Identity;

namespace FuzzyPaws2.Interfaces
{
    public interface IUserService
    {
        Task<UserIndexViewModel> GetUsersAsync();
        Task<UserCreateViewModel> PrepareCreateViewModelAsync();
        IdentityUser GetById(string id);
        Task<Result> DeleteAsync(UserCreateViewModel model);
        Task<Result> EditAsync(UserCreateViewModel model);

    }
}
