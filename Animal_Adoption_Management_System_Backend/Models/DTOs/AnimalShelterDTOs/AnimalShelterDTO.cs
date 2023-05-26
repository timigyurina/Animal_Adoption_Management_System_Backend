using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ShelterDTOs;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalShelterDTOs
{
    public class AnimalShelterDTO
    {
        public int Id { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public AnimalDTO Animal { get; set; }
        public ShelterDTOWithAddress Shelter { get; set; }
    }
}
