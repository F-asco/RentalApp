
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentalApp.Models
{
    public class EquipmentCategory
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Equipment> Equipment { get; set; }
    }
}
