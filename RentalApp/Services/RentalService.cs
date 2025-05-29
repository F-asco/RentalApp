using Microsoft.EntityFrameworkCore;
using RentalApp.Data;
using RentalApp.DTOs;
using RentalApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalApp.Services
{
    public class RentalService
    {
        private readonly ApplicationDbContext _db;
        public RentalService(ApplicationDbContext db) => _db = db;

        public async Task<List<Rental>> GetAllAsync() =>
            await _db.Rentals
                .Include(r => r.Equipment)
                .Include(r => r.User)
                .ToListAsync();

        public async Task RentAsync(RentalDto dto)
        {
            var rent = new Rental
            {
                EquipmentId = dto.EquipmentId,
                UserId = dto.UserId,
                RentedAt = DateTime.UtcNow
            };
            _db.Rentals.Add(rent);
            await _db.SaveChangesAsync();
        }

        public async Task ReturnAsync(int rentalId)
        {
            var rent = await _db.Rentals.FindAsync(rentalId);
            if (rent != null && rent.ReturnedAt == null)
            {
                rent.ReturnedAt = DateTime.UtcNow;
                _db.Rentals.Update(rent);
                await _db.SaveChangesAsync();
            }
        }
    }
}
