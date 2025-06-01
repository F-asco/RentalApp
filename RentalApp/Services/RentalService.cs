using Microsoft.EntityFrameworkCore;
using RentalApp.Data;
using RentalApp.DTOs;
using RentalApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalApp.Services
{
    public class RentalService
    {
        private readonly ApplicationDbContext _db;

        public RentalService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Rental>> GetAllAsync()
        {
            return await _db.Rentals
                .Include(r => r.Equipment)
                .Include(r => r.User)  
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Rental>> GetByUserIdAsync(string userId)
        {
            return await _db.Rentals
                .Include(r => r.Equipment)
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task RentAsync(RentalDto dto)
        {
            var rent = new Rental
            {
                EquipmentId = dto.EquipmentId,
                UserId = dto.UserId,
                RentDate = dto.RentDate,
                DueDate = dto.DueDate,
                IsReturned = dto.IsReturned,
                ReturnedAt = dto.ReturnedAt
            };

            var equipment = await _db.Equipment.FindAsync(dto.EquipmentId);
            if (equipment != null && equipment.QuantityAvailable > 0)
            {
                equipment.QuantityAvailable--;
            }

            _db.Rentals.Add(rent);
            await _db.SaveChangesAsync();
        }

        public async Task ReturnAsync(int rentalId)
        {
            var rental = await _db.Rentals.Include(r => r.Equipment).FirstOrDefaultAsync(r => r.Id == rentalId);
            if (rental != null && !rental.IsReturned)
            {
                rental.IsReturned = true;
                rental.ReturnedAt = System.DateTime.Now;

                if (rental.Equipment != null)
                {
                    rental.Equipment.QuantityAvailable++;
                }

                await _db.SaveChangesAsync();
            }
        }

        public async Task ApproveAsync(int id)
        {
            var rental = await _db.Rentals.FindAsync(id);
            if (rental != null && !rental.IsApproved && !rental.IsCanceled)
            {
                rental.IsApproved = true;
                await _db.SaveChangesAsync();
            }
        }

        public async Task CancelAsync(int id)
        {
            var rental = await _db.Rentals.FindAsync(id);
            if (rental != null && !rental.IsReturned)
            {
                rental.IsCanceled = true;
                await _db.SaveChangesAsync();
            }
        }

    }
}