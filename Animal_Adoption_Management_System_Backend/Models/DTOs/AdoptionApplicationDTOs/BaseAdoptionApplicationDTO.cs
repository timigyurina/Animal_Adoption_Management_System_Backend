using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionApplicationDTOs
{
    public abstract class BaseAdoptionApplicationDTO
    {
        [Required]
        public DateTime ApplicationDate { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
