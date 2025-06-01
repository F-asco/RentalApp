using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentalApp.Models;

namespace RentalApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Equipment> Equipment { get; set; }

        public DbSet<EquipmentCategory> EquipmentCategories { get; set; }
        public DbSet<Rental> Rentals { get; set; }
    }
}