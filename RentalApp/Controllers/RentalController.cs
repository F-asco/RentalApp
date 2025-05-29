using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalApp.DTOs;
using RentalApp.Services;
using RentalApp.Models;

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

        // Wyświetla listę wszystkich wypożyczeń
        public async Task<IActionResult> Index()
        {
            var rentals = await _rentalService.GetAllAsync();
            return View(rentals);
        }

        // Pokazuje formularz wypożyczenia (lista dostępnego sprzętu)
        public async Task<IActionResult> Rent()
        {
            var equipmentList = await _equipmentService.GetAllAsync();
            return View(equipmentList);
        }

        // Obsługuje akcję wypożyczenia
        [HttpPost]
        public async Task<IActionResult> Rent(int equipmentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dto = new RentalDto
            {
                EquipmentId = equipmentId,
                UserId = userId
            };
            await _rentalService.RentAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // Zwraca sprzęt
        [HttpPost]
        public async Task<IActionResult> Return(int id)
        {
            await _rentalService.ReturnAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
