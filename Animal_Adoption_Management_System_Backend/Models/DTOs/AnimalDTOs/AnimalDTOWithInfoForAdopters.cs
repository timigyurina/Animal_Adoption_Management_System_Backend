using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalBreedDTOS;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ShelterDTOs;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs
{
    public class AnimalDTOWithInfoForAdopters : BaseAnimalDTO
    {
        public int Id { get; set; }
        public AnimalBreedDTO Breed { get; set; }
        public ShelterDTOWithDetails LatestShelter { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime? ExitDate { get; set; }

        public virtual ICollection<ImageDTO> Images { get; set; }
    }
}
