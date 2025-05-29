using Microsoft.EntityFrameworkCore;
using RentalApp.Data;
using RentalApp.DTOs;
using RentalApp.Models;
using RentalApp.DTOs;
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

        public async Task CreateAsync(EquipmentDto dto)
        {
            var e = new Equipment { Name = dto.Name, Description = dto.Description };
            _db.Equipment.Add(e);
            await _db.SaveChangesAsync();
        }

        // Edit, Delete – analogicznie
    }
}