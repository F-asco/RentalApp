using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalApp.DTOs;
using RentalApp.Models;
using RentalApp.Services;
using System.Threading.Tasks;

namespace RentalApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _auth;

        public AccountController(AuthService auth) => _auth = auth;

        [HttpGet] public IActionResult Register() => View();
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var result = await _auth.RegisterAsync(dto);
            if (result.Succeeded) return RedirectToAction("Login");
            foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
            return View(dto);
        }

        [HttpGet] public IActionResult Login() => View();
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var res = await _auth.LoginAsync(dto);
            if (res.Succeeded) return RedirectToAction("Index", "Home");
            ModelState.AddModelError("", "Błędny login lub hasło");
            return View(dto);
        }

        public async Task<IActionResult> Logout()
        {
            await _auth.LogoutAsync();
            return RedirectToAction("Login");
        }
    }
}