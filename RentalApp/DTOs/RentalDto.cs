using System;
using System.ComponentModel.DataAnnotations;

namespace RentalApp.DTOs
{
    public class RentalDto
    {
        public int Id { get; set; }

        [Required]
        public int EquipmentId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime RentDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime DueDate { get; set; }

        public bool IsReturned { get; set; }

        public DateTime? ReturnedAt { get; set; }
    }
}
