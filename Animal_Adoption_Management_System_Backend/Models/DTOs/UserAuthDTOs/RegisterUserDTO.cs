using Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs;
using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.UserAuthDTOs
{
    public class RegisterUserDTO : BaseUserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "Password should be min {2} max {1} characters long", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
