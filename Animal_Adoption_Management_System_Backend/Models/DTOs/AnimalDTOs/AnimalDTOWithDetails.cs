using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalBreedDTOS;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalShelterDTOs;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs
{
    public class AnimalDTOWithDetails : BaseAnimalDTO
    {
        public int Id { get; set; }
        public AnimalBreedDTO Breed { get; set; }
        //public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<AnimalShelterDTO> AnimalShelters { get; set; }
        //public virtual ICollection<AdoptionApplication> AdoptionApplications { get; set; }
        //public virtual ICollection<AdoptionContract> AdoptionContracts { get; set; }
    }
}
