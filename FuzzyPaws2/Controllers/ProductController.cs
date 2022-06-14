using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FuzzyPaws2.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            //var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View();
        }
    }
}
