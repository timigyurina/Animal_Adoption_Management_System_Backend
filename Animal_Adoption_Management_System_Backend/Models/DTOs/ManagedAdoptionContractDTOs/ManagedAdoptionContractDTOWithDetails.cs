using Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionContractDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.ManagedAdoptionContractDTOs
{
    public class ManagedAdoptionContractDTOWithDetails
    {
        public int Id { get; set; }

        public AdoptionContractDTO Contract { get; set; }
        public UserDTO Manager { get; set; }
    }
}
