namespace Animal_Adoption_Management_System_Backend.Models.DTOs.ShelterDTOs
{
    public class ShelterDTO : BaseShelterDTO
    {
        public int Id { get; set; }
    }
    //public class ShelterDTOWithDetails : BaseShelterDTO
    //{ 
    //    public int Id { get; set; }

    //    public AddressDTO Address { get; set; }
    //    public User? ContactPerson { get; set; }
    //    public virtual ICollection<AnimalShelter> Animals { get; set; }
    //    public virtual ICollection<Donation> Donations { get; set; }
    //}
}
