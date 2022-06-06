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

        public UserService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserIndexViewModel> GetUsersAsync()
        {
            var model = new UserIndexViewModel()
            {
                AllUsers = await _context.AspNetUsers.ToListAsync()
            };

            return model;
        }

        public async Task<UserCreateViewModel> PrepareCreateViewModelAsync()
        {
            var model = new UserCreateViewModel();

            return model;
        }

        public IdentityUser GetById(int id)
        {
            return _context.AspNetUsers.FirstOrDefault(x => x.Id == id.ToString());
        }

        public async Task<Result> DeleteAsync(UserCreateViewModel model)
        {
            var mappedUser = _mapper.Map<IdentityUser>(model);

            _context.AspNetUsers.Remove(mappedUser);
            _context.SaveChanges();

            return Result.Success(mappedUser);
        }
    }
}
