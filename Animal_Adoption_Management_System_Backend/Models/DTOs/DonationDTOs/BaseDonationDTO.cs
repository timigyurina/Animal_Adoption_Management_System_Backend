using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.DonationDTOs
{
    public abstract class BaseDonationDTO
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
