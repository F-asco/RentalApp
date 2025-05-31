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

        [Authorize(Roles = "Admin,Pracownik")]
        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(EquipmentDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _svc.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Pracownik")]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _svc.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Pracownik")]
        public async Task<IActionResult> Edit(int id, EquipmentDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _svc.UpdateAsync(id, dto);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Pracownik")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _svc.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,Pracownik")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _svc.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}