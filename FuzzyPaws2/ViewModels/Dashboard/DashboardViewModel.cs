using FuzzyPaws2.Models;
using Microsoft.AspNetCore.Identity;

namespace FuzzyPaws2.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public int AppointmentsCount { get; set; }
        public int AvailablePetsCount { get; set; }

        public IEnumerable<Appointment>? App { get; set; }
        //public IEnumerable<Pet>? AllPets { get; set; }
        //public IEnumerable<IdentityUser>? AllUsers { get; set; }

    }
}
