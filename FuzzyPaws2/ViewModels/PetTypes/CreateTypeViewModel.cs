using FuzzyPaws2.ViewModels.Images;

namespace FuzzyPaws2.ViewModels.PetTypes
{
    public class CreateTypeViewModel : EditImageViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
