using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionApplicationDTOs
{
    public class CreateAdoptionApplicationDTO 
    {
        [Required]
        public int AnimalId { get; set; }
    }
}
