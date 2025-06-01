using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalApp.Data;
using RentalApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RentalApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        // ✅ Jeden wspólny konstruktor z trzema zależnościami
        public AdminController(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);

            ViewBag.User = user;
            ViewBag.UserRoles = userRoles;
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (!await _userManager.IsInRoleAsync(user, role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            return RedirectToAction("ManageRoles", new { userId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (await _userManager.IsInRoleAsync(user, role))
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }
            return RedirectToAction("ManageRoles", new { userId });
        }

        public IActionResult Dashboard()
        {
            var mostRented = _db.Rentals
                .Where(r => r.IsApproved && !r.IsCanceled)
                .GroupBy(r => r.Equipment.Name)
                .OrderByDescending(g => g.Count())
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .Take(5)
                .ToList();

            var total = _db.Rentals.Count();
            var onTime = _db.Rentals.Count(r => r.ReturnedAt <= r.DueDate);
            var late = _db.Rentals.Count(r => r.ReturnedAt > r.DueDate);

            ViewBag.MostRented = mostRented;
            ViewBag.OnTimePercent = total > 0 ? (onTime * 100) / total : 0;
            ViewBag.LatePercent = total > 0 ? (late * 100) / total : 0;

            return View();
        }

        public IActionResult Categories() => View(_db.EquipmentCategories.ToList());

        public IActionResult CreateCategory()
        {
            return View(new EquipmentCategory()); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(EquipmentCategory cat)
        {
            if (!ModelState.IsValid)
                return View(cat);

            // Unikalność
            if (_db.EquipmentCategories.Any(c => c.Name == cat.Name))
            {
                ModelState.AddModelError("Name", "Taka kategoria już istnieje.");
                return View(cat);
            }

            _db.EquipmentCategories.Add(cat);
            await _db.SaveChangesAsync();
            return RedirectToAction("Categories");
        }

        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            var category = _db.EquipmentCategories.Find(id);
            if (category == null) return NotFound();
            return View("CreateCategory", category);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(EquipmentCategory cat)
        {
            if (!ModelState.IsValid)
                return View("CreateCategory", cat);

            // Walidacja unikalności
            bool exists = _db.EquipmentCategories
                .Any(c => c.Id != cat.Id && c.Name.ToLower() == cat.Name.ToLower());

            if (exists)
            {
                ModelState.AddModelError("Name", "Taka kategoria już istnieje.");
                return View("CreateCategory", cat);
            }

            _db.EquipmentCategories.Update(cat);
            await _db.SaveChangesAsync();
            return RedirectToAction("Categories");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = _db.EquipmentCategories
                .Include(c => c.Equipment)
                .FirstOrDefault(c => c.Id == id);

            if (category == null) return NotFound();

            if (category.Equipment.Any())
            {
                TempData["Error"] = "Nie można usunąć kategorii, do której przypisano sprzęt.";
                return RedirectToAction("Categories");
            }

            _db.EquipmentCategories.Remove(category);
            await _db.SaveChangesAsync();
            return RedirectToAction("Categories");
        }
    }
}