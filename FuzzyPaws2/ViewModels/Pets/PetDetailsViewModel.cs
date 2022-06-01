namespace FuzzyPaws2.ViewModels.Pets
{
    public class PetDetailsViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool IsSold { get; set; }

        public string PetBreedName { get; set; }
        public string PetTypeName { get; set; }
    }
}
