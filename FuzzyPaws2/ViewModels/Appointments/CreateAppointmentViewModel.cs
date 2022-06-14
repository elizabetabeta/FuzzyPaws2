using FuzzyPaws2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FuzzyPaws2.ViewModels.Appointments
{
    public class CreateAppointmentViewModel
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Description { get; set; }
        public int ExpectedPrice { get; set; }
        public int FinalPrice { get; set; }
        public int MyPetId { get; set; }
        public Status status { get; set; }
        public IEnumerable<SelectListItem> MyPets { get; set; }

    }
}
