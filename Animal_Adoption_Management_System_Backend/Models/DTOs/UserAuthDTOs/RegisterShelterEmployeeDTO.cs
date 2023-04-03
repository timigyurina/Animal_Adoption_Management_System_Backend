using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.UserAuthDTOs
{
    public class RegisterShelterEmployeeDTO : RegisterUserDTO
    {
        [Required]
        public int ShelterId { get; set; }
        [Required]
        public bool IsContactOfShelter { get; set; }
    }
}
