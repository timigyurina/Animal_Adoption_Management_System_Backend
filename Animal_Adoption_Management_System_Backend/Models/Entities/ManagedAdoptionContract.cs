namespace Animal_Adoption_Management_System_Backend.Models.Entities
{
    public class ManagedAdoptionContract
    {
        public int Id { get; set; }

        public AdoptionContract Contract { get; set; }
        public User Manager { get; set; }
    }
}
