using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalApp.DTOs;
using RentalApp.Services;
using RentalApp.DTOs;
using System.Threading.Tasks;

namespace RentalApp.Controllers
{
    [Authorize]
    public class EquipmentController : Controller
    {
        private readonly EquipmentService _svc;
        public EquipmentController(EquipmentService svc) => _svc = svc;

        public async Task<IActionResult> Index()
        {
            var list = await _svc.GetAllAsync();
            return View(list);
        }

        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(EquipmentDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _svc.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        // Edit/Delete analogicznie
    }
}