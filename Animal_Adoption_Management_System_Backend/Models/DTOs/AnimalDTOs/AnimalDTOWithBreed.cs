using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalBreedDTOS;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs
{
    public class AnimalDTOWithBreed : BaseAnimalDTO
    {
        public int Id { get; set; }
        public AnimalBreedDTO Breed { get; set; }
    }
}
