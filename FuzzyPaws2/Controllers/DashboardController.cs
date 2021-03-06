using FuzzyPaws2.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyPaws2.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        public IActionResult _Dashboard()
        {
            var model = _dashboardService.GetAppointmentsAsync();
            return PartialView(model);
        }
    }
}
