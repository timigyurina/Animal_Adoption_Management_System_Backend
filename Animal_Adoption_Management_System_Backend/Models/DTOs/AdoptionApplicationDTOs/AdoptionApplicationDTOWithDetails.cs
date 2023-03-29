using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionApplicationDTOs
{
    public class AdoptionApplicationDTOWithDetails : BaseAdoptionApplicationDTO
    {
        public int Id { get; set; }

        public AnimalDTO Animal { get; set; }
        public UserDTO Applier { get; set; }
    }
}
