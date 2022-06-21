using FuzzyPaws2.ViewModels.Dashboard;

namespace FuzzyPaws2.Interfaces
{
    public interface IDashboardService
    {
        //Task<DashboardViewModel> GetPetsAsync();
        Task<DashboardViewModel> GetAppointmentsAsync();
        //Task<DashboardViewModel> GetUserAsync();

    }
}
