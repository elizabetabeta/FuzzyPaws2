using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FuzzyPaws2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IDashboardService _dashboardService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IDashboardService dashboardService)
        {
            _logger = logger;
            _context = context;
            _dashboardService = dashboardService;
        }


        public IActionResult Index()
        {
            var indexViewModel = new HomeIndexViewModel();
            indexViewModel.dashboardViewModel = _dashboardService.GetAppointmentsAsync();
            indexViewModel.LatestApps = _context.Appointments.OrderByDescending(x => x.Id).Take(10).ToList();
            indexViewModel.LatestPets = _context.Pets.Where(x => x.IsSold == false)
                                                     .OrderByDescending(x => x.Id)
                                                     .Take(4)
                                                     .ToList();
            indexViewModel.LatestMyPets = _context.MyPets.OrderByDescending(x => x.Id)
                                                         .Take(5)
                                                         .ToList();

            return View(indexViewModel);
        }

        public IActionResult DashboardPins()
        {
            var model = _dashboardService.GetAppointmentsAsync();
            return PartialView("_DashboardPins", model);
        }

        [HttpPost]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}