using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs
{
    public class ImageDTOWithDetails : BaseImageDTO
    {
        public int Id { get; set; }

        public UserDTO Uploader { get; set; }
        public AnimalDTO Animal { get; set; }
    }
}
