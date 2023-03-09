using Animal_Adoption_Management_System_Backend.Models.Enums;

namespace Animal_Adoption_Management_System_Backend.Models.Entities
{
    public class Donation
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public DonationStatus Status { get; set; }

        public User Donator { get; set; }
        public Shelter Shelter { get; set; }
    }
}
