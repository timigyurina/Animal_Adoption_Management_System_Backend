using Animal_Adoption_Management_System_Backend.Models.DTOs.AddressDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalShelterDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.DonationDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.ShelterDTOs
{
    public class ShelterDTOWithDetails : BaseShelterDTO
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }


        public AddressDTO Address { get; set; }
        public virtual ICollection<AnimalShelterDTO> Animals { get; set; }
        public virtual ICollection<DonationDTO> Donations { get; set; }
        public virtual ICollection<UserDTO> Employees { get; set; }
    }
}
