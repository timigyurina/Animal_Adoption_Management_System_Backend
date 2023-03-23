using Animal_Adoption_Management_System_Backend.Models.DTOs.AddressDTOs;
using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.ShelterDTOs
{
    public class CreateShelterDTO
    {
        [Required]
        public CreateAddressDTO Address { get; set; }
        public int UserId { get; set; }
    }
}
