using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.AddressDTOs
{
    public class BaseAddressDTO
    {
        [Required]
        public string Country { get; set; }

        [Required]
        public string PostalCode { get; set; }

        public string Region { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string AddressLineOne { get; set; }

        public string AddressLineTwo { get; set; }
    }
}
