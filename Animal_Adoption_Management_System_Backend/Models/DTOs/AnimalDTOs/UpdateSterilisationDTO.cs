using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs
{
    public class UpdateSterilisationDTO
    {
        [Required]
        public DateTime SterilisationDate { get; set; }
    }
}
