using AutoMapper;
using FuzzyPaws2.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyPaws2.Controllers
{
    public class RoleController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index()
        {
            var model = await _roleService.GetRolesAsync();
            return View(model);
        }
    }
}
