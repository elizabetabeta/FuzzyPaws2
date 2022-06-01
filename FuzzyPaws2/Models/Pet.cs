using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuzzyPaws2.Models
{
    public class Pet
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public bool IsSold { get; set; }

        [ForeignKey("PetType")]
        public int PetTypeId { get; set; }
        public PetType PetType { get; set; }

        [ForeignKey("PetBreed")]
        public int PetBreedId { get; set; }
        public PetBreed PetBreed { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

    }
}
