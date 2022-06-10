using System.ComponentModel.DataAnnotations;

namespace FuzzyPaws2.ViewModels.Images
{
    public class UploadImageViewModel
    {
        [Display(Name = "Picture")]
        public IFormFile Picture { get; set; }
    }
}
