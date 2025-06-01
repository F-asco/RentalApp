using System.ComponentModel.DataAnnotations;

namespace RentalApp.DTOs
{
    public class EquipmentDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public int    QuantityAvailable { get; set; }
        public bool IsAvailable => QuantityAvailable > 0;
    }
}