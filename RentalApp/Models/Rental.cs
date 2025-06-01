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
        public ApplicationUser User { get; set; }

        [Required]
        public DateTime RentDate { get; set; } = DateTime.Now;

        public DateTime DueDate { get; set; } 

        public bool IsReturned { get; set; } = false;

        public DateTime? ReturnedAt { get; set; }
        public bool IsApproved { get; set; } = false;
        public bool IsCanceled { get; set; } = false;

        [NotMapped]
        public bool IsOverdue => !IsReturned && DueDate < DateTime.Now;
    }
}
