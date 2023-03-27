using Animal_Adoption_Management_System_Backend.Models.DTOs.DonationDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs
{
    public class UserDTOWithDetails : BaseUserDTO
    {
        public virtual ICollection<DonationDTO> Donations { get; set; }
        public virtual ICollection<AdoptionApplication> AdoptionApplications { get; set; }
        public virtual ICollection<AdoptionContract> AdoptionsContracts { get; set; }
        public virtual ICollection<ManagedAdoptionContract> ManagedAdoptionsContracts { get; set; }
    }
}
