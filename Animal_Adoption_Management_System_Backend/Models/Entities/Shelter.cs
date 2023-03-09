namespace Animal_Adoption_Management_System_Backend.Models.Entities
{
    public class Shelter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

        public Address Address { get; set; }
        public User? ContactPerson { get; set; }
        public virtual ICollection<AnimalShelter> Animals { get; set; }
        public virtual ICollection<Donation> Donations { get; set; }
    }
}
