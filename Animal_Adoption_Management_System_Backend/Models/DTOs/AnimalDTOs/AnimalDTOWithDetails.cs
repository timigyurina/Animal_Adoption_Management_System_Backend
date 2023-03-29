using Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionApplicationDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionContractDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalBreedDTOS;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalShelterDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs
{
    public class AnimalDTOWithDetails : BaseAnimalDTO
    {
        public int Id { get; set; }

        public AnimalBreedDTO Breed { get; set; }
        public virtual ICollection<ImageDTO> Images { get; set; }
        public virtual ICollection<AnimalShelterDTO> AnimalShelters { get; set; }
        public virtual ICollection<AdoptionApplicationDTO> AdoptionApplications { get; set; }
        public virtual ICollection<AdoptionContractDTO> AdoptionContracts { get; set; }
    }
}
