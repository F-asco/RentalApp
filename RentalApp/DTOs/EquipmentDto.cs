using System.ComponentModel.DataAnnotations;

namespace RentalApp.DTOs
{
    public class EquipmentDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}