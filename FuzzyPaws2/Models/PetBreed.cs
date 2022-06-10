using System.ComponentModel.DataAnnotations;

namespace FuzzyPaws2.Models
{
    public class PetBreed
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Image { get; set; }
    }
}
