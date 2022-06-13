using AutoMapper;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.ViewModels.Appointments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyPaws2.Controllers
{
    public class AppointmentController : Controller
    { 
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public AppointmentController(IAppointmentService appointmentService,
                                     IMapper mapper,
                                     ApplicationDbContext context)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _appointmentService.GetAppointmentsAsync();
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _appointmentService.MakeAnAppointmentAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateAppointmentViewModel model)
        {
            var day = model.Time;
            if (day < DateTime.Now)
            {
                TempData["error"] = "You can't choose past date. Please select a valid time.";
            }
            else if (_context.Appointments.Any(x => x.Time == model.Time))
            {
                TempData["error"] = "Someone already booked at this time. Please check which time is available.";
            }
            else if (day.DayOfWeek == DayOfWeek.Sunday || day.DayOfWeek == DayOfWeek.Saturday)
            {
                TempData["error"] = "Invalid date. Cannot select Saturday or Sunday.";
            } else if (day.Hour < 8 || day.Hour > 16)
            {
                TempData["error"] = "Invalid time. Working hour starts at 8AM and ends at 4PM.";
            }
            else
            {
                _appointmentService.CreateAsync(model);

                TempData["success"] = "Appointment booked successfully. Please wait for vet's confirmation.";
            }

            return RedirectToAction("Index");

        }

        [Authorize(Roles = "Vet")]
        public IActionResult Status(int id)
        {
            var appointment = _appointmentService.GetById(id);

            var mappedApp = _mapper.Map<CreateAppointmentViewModel>(appointment);

            return View(mappedApp);

        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Status(int id, CreateAppointmentViewModel model)
        {
            var app = _appointmentService.GetById(id);

            app.status = model.status;
 
            _context.Update(app);
            _context.SaveChanges();

            TempData["success"] = "Appointment status changed successfully";

            return RedirectToAction("Index");
        }
    }
}
