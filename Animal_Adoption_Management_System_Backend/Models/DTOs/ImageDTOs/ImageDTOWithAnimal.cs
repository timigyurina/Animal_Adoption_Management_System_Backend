using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs
{
    public class ImageDTOWithAnimal : BaseImageDTO
    {
        public int Id { get; set; }
        public DateTime DateTaken { get; set; }
        public string ImagePath { get; set; }

        public AnimalDTO Animal { get; set; }
    }
}
