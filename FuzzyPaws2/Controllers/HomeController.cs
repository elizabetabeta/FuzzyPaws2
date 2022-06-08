using FuzzyPaws2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FuzzyPaws2.Controllers
{
    public class AccountController : Controller
    {

        public AccountController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}