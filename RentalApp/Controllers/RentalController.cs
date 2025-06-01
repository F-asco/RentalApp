using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalApp.DTOs;
using RentalApp.Services;
using System;

namespace RentalApp.Controllers
{
    [Authorize]
    public class RentalController : Controller
    {
        private readonly RentalService _rentalService;
        private readonly EquipmentService _equipmentService;

        public RentalController(RentalService rentalService, EquipmentService equipmentService)
        {
            _rentalService = rentalService;
            _equipmentService = equipmentService;
        }

        
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Pracownik"))
            {
                var allRentals = await _rentalService.GetAllAsync();
                return View(allRentals);
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userRentals = await _rentalService.GetByUserIdAsync(userId);
                return View(userRentals);
            }
        }

        
        public async Task<IActionResult> Rent()
        {
            var equipmentList = await _equipmentService.GetAllAsync();
            return View(equipmentList);
        }

        
        [HttpPost]
        [Authorize(Roles = "Użytkownik,Pracownik,Admin")]
        public async Task<IActionResult> Create(int equipmentId, int rentalPeriod)
        {
            var equipment = await _equipmentService.GetByIdAsync(equipmentId);
            if (equipment == null || equipment.QuantityAvailable <= 0)
            {
                TempData["Error"] = "Sprzęt jest niedostępny.";
                return RedirectToAction("Rent");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dto = new RentalDto
            {
                EquipmentId = equipmentId,
                UserId = userId,
                RentDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(rentalPeriod),
                IsReturned = false
            };

            await _rentalService.RentAsync(dto);
            return RedirectToAction("MyRentals");
        }

        
        public async Task<IActionResult> MyRentals()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rentals = await _rentalService.GetByUserIdAsync(userId);
            return View("Index", rentals);
        }

        
        [HttpPost]
        public async Task<IActionResult> Return(int id)
        {
            await _rentalService.ReturnAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Pracownik")]
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            await _rentalService.ApproveAsync(id);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,Pracownik")]
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            await _rentalService.CancelAsync(id);
            return RedirectToAction("Index");
        }
    }
}