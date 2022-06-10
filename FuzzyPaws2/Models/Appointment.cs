using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuzzyPaws2.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        [ForeignKey("MyPets")]
        public int MyPetId { get; set; }
        public MyPet MyPets { get; set; }

    }
}
