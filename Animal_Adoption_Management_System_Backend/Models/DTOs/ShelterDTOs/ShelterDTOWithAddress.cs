using Animal_Adoption_Management_System_Backend.Models.DTOs.AddressDTOs;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.ShelterDTOs
{
    public class ShelterDTOWithAddress : BaseShelterDTO
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public AddressDTO Address { get; set; }
    }
}
