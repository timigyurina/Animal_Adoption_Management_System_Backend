using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs
{
    public class CreateImageDTO : BaseImageDTO
    {
        [Required]
        public int UploaderId { get; set; }
        [Required]
        public int AnimalId { get; set; }
    }
}
