using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs
{
    public class CreateImageDTO : BaseImageDTO
    {
        [Required]
        public IFormFile Image { get; set; }

        [Required]
        public int AnimalId { get; set; }
        public DateTime? DateTaken { get; set; }
    }
}
