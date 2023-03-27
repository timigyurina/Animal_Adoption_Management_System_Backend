using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.DonationDTOs
{
    public class CreateDonationDTO : BaseDonationDTO
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public int ShelterId { get; set; }
    }
}
