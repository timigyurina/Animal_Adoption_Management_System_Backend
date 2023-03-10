using Animal_Adoption_Management_System_Backend.Models.Enums;

namespace Animal_Adoption_Management_System_Backend.Models.Entities
{
    public class AnimalBreed
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AnimalType Type { get; set; }
    }
}
