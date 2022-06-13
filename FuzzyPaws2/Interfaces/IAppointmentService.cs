using FuzzyPaws2.Core.Model;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.Appointments;

namespace FuzzyPaws2.Interfaces
{
    public interface IAppointmentService
    {
        Task<AppointmentsIndexViewModel> GetAppointmentsAsync();
        Task<CreateAppointmentViewModel> MakeAnAppointmentAsync();
        Task<Result> CreateAsync(CreateAppointmentViewModel model);
        Task<Result> EditAsync(int id, CreateAppointmentViewModel model);
        Appointment GetById(int id);
    }
}
