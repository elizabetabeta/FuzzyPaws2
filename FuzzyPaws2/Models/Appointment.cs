using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuzzyPaws2.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public string Description { get; set; }
        public int Price { get; set; }

        public Status status { get; set; }

        [ForeignKey("MyPets")]
        public int MyPetId { get; set; }
        public MyPet MyPets { get; set; }

    }
    public enum Status
    {
        Waiting, Confirmed, Rejected, Finished
    }
}
