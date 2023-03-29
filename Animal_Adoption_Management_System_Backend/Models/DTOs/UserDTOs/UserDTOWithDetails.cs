using Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionApplicationDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionContractDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.DonationDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ManagedAdoptionContractDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ShelterDTOs;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs
{
    public class UserDTOWithDetails : BaseUserDTO
    {
        public virtual ICollection<DonationDTO> Donations { get; set; }
        public virtual ICollection<ImageDTO> Images { get; set; }
        public virtual ICollection<ShelterDTO> Shelters { get; set; }
        public virtual ICollection<AdoptionApplicationDTO> AdoptionApplications { get; set; }
        public virtual ICollection<AdoptionContractDTO> AdoptionsContracts { get; set; }
        public virtual ICollection<ManagedAdoptionContractDTOWithDetails> ManagedAdoptionsContracts { get; set; }
    }
}
