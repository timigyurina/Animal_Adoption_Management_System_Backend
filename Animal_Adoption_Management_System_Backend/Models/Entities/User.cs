using Microsoft.AspNetCore.Identity;

namespace Animal_Adoption_Management_System_Backend.Models.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOfBirth { get; set; }   

        public virtual ICollection<Donation> Donations { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Shelter> Shelters { get; set; }
        public virtual ICollection<AdoptionApplication> AdoptionApplications { get; set; }
        public virtual ICollection<AdoptionContract> AdoptionsContracts { get; set; }
        public virtual ICollection<ManagedAdoptionContract> ManagedAdoptionsContracts { get; set; } // if user is an employer at a shelter
    }
}
