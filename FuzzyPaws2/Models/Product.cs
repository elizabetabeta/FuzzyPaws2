using System.ComponentModel.DataAnnotations;

namespace FuzzyPaws2.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Description { get; set; }
        //public int Quantity { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        //public int Discount { get; set; }
       
    }
}
