using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs
{
    public abstract class BaseUserDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required] 
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
