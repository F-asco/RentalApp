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

        public int CategoryId { get; set; }

        public EquipmentCategory Category { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }

        [Range(0, int.MaxValue)]
        public int QuantityAvailable { get; set; }

        public bool IsAvailable => QuantityAvailable > 0;

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;



    }
}