using FuzzyPaws2.Models;

namespace FuzzyPaws2.ViewModels.Appointments
{
    public class CreateAppointmentViewModel
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Description { get; set; }
        public int MyPetId { get; set; }
        public Status status { get; set; }
    }
}
