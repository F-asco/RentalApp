using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentalApp.Models
{
    public class Equipment
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        // Navigation property - list of rentals for this equipment
        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}