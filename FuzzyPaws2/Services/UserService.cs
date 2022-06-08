using AutoMapper;
using FuzzyPaws2.Core.Model;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FuzzyPaws2.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(ApplicationDbContext context, 
                           IMapper mapper, 
                          UserManager<IdentityUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<UserIndexViewModel> GetAdminAsync()
        {
            var model = new UserIndexViewModel()
            {
                AllUsers = await _userManager.GetUsersInRoleAsync("ADMIN")
            };

            return model;
        }
        public async Task<UserIndexViewModel> GetVetAsync()
        {
            var model = new UserIndexViewModel()
            {
                AllUsers = await _userManager.GetUsersInRoleAsync("VET")
            };

            return model;
        }
        public async Task<UserIndexViewModel> GetUserAsync()
        {
            var model = new UserIndexViewModel()
            {
                AllUsers = await _userManager.GetUsersInRoleAsync("USER")
            };

            return model;
        }

        public async Task<UserCreateViewModel> PrepareCreateViewModelAsync()
        {
            var model = new UserCreateViewModel();

            return model;
        }

        public IdentityUser GetById(string id)
        {
            return _context.AspNetUsers.FirstOrDefault(x => x.Id == id);
        }

    }
}
