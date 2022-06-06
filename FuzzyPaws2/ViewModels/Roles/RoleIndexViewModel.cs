using Microsoft.AspNet.Identity.EntityFramework;

namespace FuzzyPaws2.ViewModels.Roles
{
    public class RoleIndexViewModel
    {
        public IEnumerable<IdentityRole> AllRoles { get; set; }
    }
}
