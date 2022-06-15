using FuzzyPaws2.Models;

namespace FuzzyPaws2.ViewModels.Appointments
{
    public class AppointmentDetailsViewModel
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Description { get; set; }
        public int ExpectedPrice { get; set; }
        public int FinalPrice { get; set; }
        public int MyPetId { get; set; }
        public string MyPetName { get; set; }
        public string MyPetImage { get; set; }
        public Status status { get; set; }
    }
}
