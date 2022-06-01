namespace FuzzyPaws2.ViewModels.Pets
{
    public class PetCreateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool IsSold { get; set; }

        public string PetBreedId { get; set; }
        public string PetTypeId { get; set; }
    }
}

