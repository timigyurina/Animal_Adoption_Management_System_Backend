using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionContractDTOs
{
    public abstract class BaseAdoptionContractDTO
    {
        [Required]
        public DateTime ContractDate { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public string Notes { get; set; }
    }
}
