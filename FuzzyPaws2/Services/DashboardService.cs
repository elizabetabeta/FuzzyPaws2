using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.ViewModels.Dashboard;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FuzzyPaws2.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardService(ApplicationDbContext context,
                                UserManager<IdentityUser> userManager)
        {

            _context = context;
            _userManager = userManager;
        }
        public async Task<DashboardViewModel> GetAppointmentsAsync()
        {
            var model = new DashboardViewModel()
            {
                AppointmentsCount = _context.Appointments.Count(),
                AvailablePetsCount = _context.Pets.Where(x=>!x.IsSold).Count()
            };

            return model;
        }

        //public async Task<DashboardViewModel> GetPetsAsync()
        //{
        //    var model = new DashboardViewModel()
        //    {
        //        AllPets = await _context.Pets
        //        .ToListAsync()
        //    };

        //    return model;
        //}

        //public async Task<DashboardViewModel> GetUserAsync()
        //{
        //    var model = new DashboardViewModel()
        //    {
        //        AllUsers = await _userManager.GetUsersInRoleAsync("USER")
        //    };

        //    return model;
        //}
    }
}
