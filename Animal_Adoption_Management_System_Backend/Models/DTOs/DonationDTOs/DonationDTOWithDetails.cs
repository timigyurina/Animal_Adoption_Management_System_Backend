using Animal_Adoption_Management_System_Backend.Models.DTOs.ShelterDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.DonationDTOs
{
    public class DonationDTOWithDetails : BaseDonationDTO
    {
        public int Id { get; set; }

        public UserDTO Donator { get; set; }
        public ShelterDTO Shelter { get; set; }
    }
}
