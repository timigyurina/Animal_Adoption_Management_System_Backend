using Animal_Adoption_Management_System_Backend.Models.Enums;

namespace Animal_Adoption_Management_System_Backend.Models.Entities
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AnimalType Type { get; set; }
        public AnimalSize Size { get; set; }
        public AnimalStatus Status { get; set; }
        public Gender Gender { get; set; }
        public bool IsSterilised { get; set; }
        public DateTime? SterilisationDate { get; set; }
        public DateTime BirthDate { get; set; }
        public AnimalColor Color { get; set; }
        public string Notes { get; set; }

        public AnimalBreed Breed { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<AnimalShelter> AnimalShelters { get; set; }
        public virtual ICollection<AdoptionApplication> AdoptionApplications { get; set; }
        public virtual ICollection<AdoptionContract> AdoptionContracts { get; set; }
    }
}
