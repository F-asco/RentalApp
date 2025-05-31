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

        public string Category { get; set; } 

        [Range(0, int.MaxValue)]
        public int QuantityAvailable { get; set; }

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}