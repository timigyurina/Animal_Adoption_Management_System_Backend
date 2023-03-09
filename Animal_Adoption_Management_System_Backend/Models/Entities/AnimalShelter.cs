namespace Animal_Adoption_Management_System_Backend.Models.Entities
{
    public class AnimalShelter
    {
        public int Id { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public Animal Animal { get; set; }
        public Shelter Shelter { get; set; }
    }
}
