using Microsoft.AspNetCore.Mvc;

namespace FuzzyPaws2.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
