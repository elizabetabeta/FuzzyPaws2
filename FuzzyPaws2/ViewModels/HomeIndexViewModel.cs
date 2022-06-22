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
    }
}
