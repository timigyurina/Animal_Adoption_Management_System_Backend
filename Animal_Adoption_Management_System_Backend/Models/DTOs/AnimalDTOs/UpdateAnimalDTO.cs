using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs
{
    public class UpdateAnimalDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Size { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public int BreedId { get; set; }
        public string Notes { get; set; }

    }
}
