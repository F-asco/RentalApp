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
        public DateTime RentedAt { get; set; }

        public DateTime? ReturnedAt { get; set; }
    }
}