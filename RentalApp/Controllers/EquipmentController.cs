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

        public IActionResult Index(string searchName, string category, bool? availableOnly, string sortOrder)
        {
            var equipment = _context.Equipment.Include(e => e.Category).AsQueryable();

            
            if (!string.IsNullOrEmpty(searchName))
                equipment = equipment.Where(e => e.Name.Contains(searchName));

            if (!string.IsNullOrEmpty(category))
                equipment = equipment.Where(e => e.Category.Name == category);

            if (availableOnly == true)
                equipment = equipment.Where(e => e.IsAvailable);

            
            ViewBag.NameSort = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.DateSort = sortOrder == "date" ? "date_desc" : "date";

            equipment = sortOrder switch
            {
                "name" => equipment.OrderBy(e => e.Name),
                "name_desc" => equipment.OrderByDescending(e => e.Name),
                "date" => equipment.OrderBy(e => e.CreatedAt),
                "date_desc" => equipment.OrderByDescending(e => e.CreatedAt),
                _ => equipment.OrderBy(e => e.Name) 
            };

            ViewBag.Categories = _context.EquipmentCategories
                .Select(c => c.Name)
                .Distinct()
                .ToList();

            return View(equipment.ToList());
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