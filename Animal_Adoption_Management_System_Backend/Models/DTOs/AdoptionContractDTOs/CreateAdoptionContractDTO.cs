using Animal_Adoption_Management_System_Backend.Models.DTOs.AddressDTOs;
using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionContractDTOs
{
    public class CreateAdoptionContractDTO : BaseAdoptionContractDTO
    {
        [Required]
        public int AnimalId { get; set; }
        [Required]
        public string ApplierId { get; set; }
        [Required]
        public string ManagerId { get; set; }
        [Required]
        public DateTime ExitDate { get; set; }

        [Required]
        public CreateAddressDTO ApplierAddress { get; set; }

    }
}
