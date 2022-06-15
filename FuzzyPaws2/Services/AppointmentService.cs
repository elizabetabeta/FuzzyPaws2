using AutoMapper;
using FuzzyPaws2.Core.Model;
using FuzzyPaws2.Data;
using FuzzyPaws2.Interfaces;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.Appointments;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Text;
using iTextSharp.text;

namespace FuzzyPaws2.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISelectListService _selectListService;

        public AppointmentService(ApplicationDbContext context,
                                  IMapper mapper,
                                  ISelectListService selectListService)
        {
            _context = context;
            _mapper = mapper;
            _selectListService = selectListService;
        }

        public Appointment GetById(int id)
        {
            return _context.Appointments.FirstOrDefault(x => x.Id == id);
        }

        public async Task<AppointmentsIndexViewModel> GetAppointmentsAsync()
        {
            var model = new AppointmentsIndexViewModel()
            {
                AllAppointments = await _context.Appointments.OrderByDescending(x => x.Time).ToListAsync()
            };

            return model;
        }

        public async Task<CreateAppointmentViewModel> MakeAnAppointmentAsync()
        {
            var model = new CreateAppointmentViewModel();
            model.MyPets = await _selectListService.GetMyPets(true, "Choose your pet");

            return model;
        }


        public async Task<Result> CreateAsync(CreateAppointmentViewModel model)
        {
            var mappedAppointment = _mapper.Map<Appointment>(model);
            mappedAppointment.status = 0;

            _context.Add(mappedAppointment);
            _context.SaveChanges();

            return Result.Success(mappedAppointment);

        }

        public async Task<Result> EditAsync(int id, CreateAppointmentViewModel model)
        {
            var mappedApp = _mapper.Map<Appointment>(model);

            _context.Update(mappedApp);
            _context.SaveChangesAsync();

            return Result.Success(mappedApp);
        }

        public async Task<AppointmentDetailsViewModel> GetAppointmentsById(int appId)
        {
            var app = _context.Appointments
                .Include(x => x.MyPets)
                .FirstOrDefault(p => p.Id == appId);

            AppointmentDetailsViewModel viewModel = _mapper.Map<AppointmentDetailsViewModel>(app);

            return viewModel;
        }

    }
}
