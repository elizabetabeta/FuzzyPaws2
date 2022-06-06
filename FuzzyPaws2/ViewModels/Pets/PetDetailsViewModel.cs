namespace FuzzyPaws2.ViewModels.Pets
{
    public class PetDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool IsSold { get; set; }
        public string PetBreedName { get; set; }
        public string PetTypeName { get; set; }
        public int PetBreedId { get; set; }
        public int PetTypeId { get; set; }
    }
}
