using Microsoft.EntityFrameworkCore;
using RentalApp.Data;
using RentalApp.DTOs;
using RentalApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalApp.Services
{
    public class EquipmentService
    {
        private readonly ApplicationDbContext _db;
        public EquipmentService(ApplicationDbContext db) => _db = db;

        public async Task<List<Equipment>> GetAllAsync() =>
            await _db.Equipment.ToListAsync();

        public async Task<EquipmentDto> GetByIdAsync(int id) 
            {
            var item = await _db.Equipment.FindAsync(id);
            if (item == null) return null;

            return new EquipmentDto
            {
                Name = item.Name,
                Description = item.Description,
                Category = item.Category,
                QuantityAvailable = item.QuantityAvailable
            };
            }
        public async Task UpdateAsync(int id, EquipmentDto dto) 
        {
            var item = await _db.Equipment.FindAsync(id);
            if (item == null) return;

            item.Name = dto.Name;
            item.Description = dto.Description;
            item.Category = dto.Category;
            item.QuantityAvailable = dto.QuantityAvailable;

            _db.Equipment.Update(item);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var item = await _db.Equipment.FindAsync(id);
            if (item == null) return;

            _db.Equipment.Remove(item);
            await _db.SaveChangesAsync();
        }
        public async Task CreateAsync(EquipmentDto dto)
        {
            var e = new Equipment { Name = dto.Name, Description = dto.Description };
            _db.Equipment.Add(e);
            await _db.SaveChangesAsync();
        }

        // Edit, Delete – analogicznie
    }
}