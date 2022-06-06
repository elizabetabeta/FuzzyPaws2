using AutoMapper;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.ViewModels.Roles;
using System.Data.Entity;

namespace FuzzyPaws2.Services
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RoleService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RoleIndexViewModel> GetRolesAsync()
        {
            throw new NotImplementedException();
        }

        //public async Task<RoleIndexViewModel> GetRolesAsync()
        //{
        //    var model = new RoleIndexViewModel()
        //    {
        //        AllRoles = await _context.AspNetRoles.ToListAsync()
        //    };

        //    return model;
        //}
    }
}
