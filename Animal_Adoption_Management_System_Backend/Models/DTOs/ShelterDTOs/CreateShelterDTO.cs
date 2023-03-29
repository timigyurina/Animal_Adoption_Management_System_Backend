using Animal_Adoption_Management_System_Backend.Models.DTOs.AddressDTOs;
using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.ShelterDTOs
{
    public class CreateShelterDTO : BaseShelterDTO
    {
        [Required]
        public CreateAddressDTO Address { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public string? ContactPersonId { get; set; }
    }
}
