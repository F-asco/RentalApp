using RentalApp.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalApp.Models
{
    public class Rental
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Equipment")]
        public int EquipmentId { get; set; }

        public Equipment Equipment { get; set; }

        [Required]
        public string UserId { get; set; }

        // Navigation property to the user
        public ApplicationUser User { get; set; }

        [Required]
        public DateTime RentedAt { get; set; }

        public DateTime? ReturnedAt { get; set; }
    }
}