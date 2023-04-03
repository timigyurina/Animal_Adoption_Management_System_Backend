using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs
{
    public class UpdateUserConnectionWithShelterDTO
    {
        public int? ShelterId { get; set; }
        [Required]
        public bool IsContactOfShelter { get; set; }

    }
}
