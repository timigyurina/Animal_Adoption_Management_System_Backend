using Animal_Adoption_Management_System_Backend.Models.DTOs.AddressDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionContractDTOs
{
    public class AdoptionContractDTOWithManagerDetails : BaseAdoptionContractDTO
    {
        public int Id { get; set; }

        public AnimalDTO Animal { get; set; }
        public UserDTO Applier { get; set; }
        public AddressDTO ApplierAddress { get; set; }
        public UserDTO Manager { get; set; }
    }
}
