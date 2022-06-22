using FuzzyPaws2.ViewModels.Dashboard;

namespace FuzzyPaws2.ViewModels
{
    public class HomeIndexViewModel
    {
        public HomeIndexViewModel()
        {
            dashboardViewModel = new DashboardViewModel();
        }
        public DashboardViewModel dashboardViewModel { get; set; }
        public List<Models.Appointment> LatestApps { get; set; }
        public List<Models.Pet> LatestPets { get; set; }
        public List<Models.MyPet> LatestMyPets { get; set; }
    }
}
