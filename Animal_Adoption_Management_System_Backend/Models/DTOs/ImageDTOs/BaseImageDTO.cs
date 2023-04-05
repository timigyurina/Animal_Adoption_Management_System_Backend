using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs
{
    public abstract class BaseImageDTO
    {
        [Required]
        public string Description { get; set; }
    }
}
