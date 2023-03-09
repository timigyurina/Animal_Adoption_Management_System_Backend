using Animal_Adoption_Management_System_Backend.Models.Enums;

namespace Animal_Adoption_Management_System_Backend.Models.Entities
{
    public class AdoptionApplication
    {
        public int Id { get; set; }
        public DateTime ApplicationDate { get; set; }
        public ApplicationStatus Status { get; set; }

        public Animal Animal { get; set; }
        public User Applier { get; set; }
    }
}
