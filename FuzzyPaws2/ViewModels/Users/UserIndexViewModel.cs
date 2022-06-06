using FuzzyPaws2.Models;
using Microsoft.AspNetCore.Identity;

namespace FuzzyPaws2.ViewModels.Users
{
    public class UserIndexViewModel
    {
        public IEnumerable<IdentityUser> AllUsers { get; set; }
    }
}