using FuzzyPaws2.ViewModels.Roles;

namespace FuzzyPaws2.Interfaces
{
    public interface IRoleService
    {
        Task<RoleIndexViewModel> GetRolesAsync();
    }
}
