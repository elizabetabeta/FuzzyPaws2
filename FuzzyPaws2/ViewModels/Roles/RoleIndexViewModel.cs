using Microsoft.AspNetCore.Identity;

namespace FuzzyPaws2.ViewModels.Roles
{
    public class RoleIndexViewModel
    {
        public IEnumerable<IdentityRole> AllRoles { get; set; }
    }
}
