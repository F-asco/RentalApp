using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalApp.Data;
using RentalApp.DTOs;
using RentalApp.Services;
using System.Threading.Tasks;

namespace RentalApp.Controllers
{
    [Authorize]
    public class EquipmentController : Controller
    {
        private readonly EquipmentService _svc;
        private readonly ApplicationDbContext _context;
        public EquipmentController(EquipmentService svc, ApplicationDbContext context)
        {
            _svc = svc;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _svc.GetAllAsync();
            return View(list);
        }

        [Authorize(Roles = "Admin,Pracownik")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(
                await _context.EquipmentCategories.ToListAsync(),
                "Id",
                "Name"
            );
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EquipmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(
                    await _context.EquipmentCategories.ToListAsync(),
                    "Id",
                    "Name",
                    dto.CategoryId 
                );
                return View(dto);
            }

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