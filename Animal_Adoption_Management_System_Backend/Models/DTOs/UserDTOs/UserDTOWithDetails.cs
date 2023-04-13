using Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionApplicationDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionContractDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.DonationDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ManagedAdoptionContractDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ShelterDTOs;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs
{
    public class UserDTOWithDetails : UserDTO
    {
        public ShelterDTOWithDetails? Shelter { get; set; }
        public virtual ICollection<DonationDTOWithDetails> Donations { get; set; }
        public virtual ICollection<ImageDTOWithDetails> Images { get; set; }
        public virtual ICollection<AdoptionApplicationDTOWithDetails> AdoptionApplications { get; set; }
        public virtual ICollection<AdoptionContractDTOWithManagerDetails> AdoptionsContracts { get; set; }
        public virtual ICollection<ManagedAdoptionContractDTOWithDetails> ManagedAdoptionsContracts { get; set; }
    }
}
