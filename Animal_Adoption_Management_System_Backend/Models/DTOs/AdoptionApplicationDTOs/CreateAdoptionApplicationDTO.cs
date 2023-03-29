using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionApplicationDTOs
{
    public class CreateAdoptionApplicationDTO : BaseAdoptionApplicationDTO
    {
        [Required]
        public int AnimalId { get; set; }
        [Required] 
        public string ApplierId { get; set; }
    }
}
