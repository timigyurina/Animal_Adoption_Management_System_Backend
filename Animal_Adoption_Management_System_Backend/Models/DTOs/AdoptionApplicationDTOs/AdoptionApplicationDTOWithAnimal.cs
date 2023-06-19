using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionApplicationDTOs
{
    public class AdoptionApplicationDTOWithAnimal : BaseAdoptionApplicationDTO
    {
        public int Id { get; set; }
        public AnimalDTOWithBreed Animal { get; set; }
    }
}
