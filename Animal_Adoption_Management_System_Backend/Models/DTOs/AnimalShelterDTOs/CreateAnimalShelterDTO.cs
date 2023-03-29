using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalShelterDTOs
{
    public class CreateAnimalShelterDTO
    {
        [Required]
        public DateTime EnrollmentDate { get; set; }

        public DateTime? ExitDate { get; set; }
        [Required]
        public int ShelterId { get; set; }
    }
}
