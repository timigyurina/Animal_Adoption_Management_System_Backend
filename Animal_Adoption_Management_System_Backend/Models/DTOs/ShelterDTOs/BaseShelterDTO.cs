namespace Animal_Adoption_Management_System_Backend.Models.DTOs.ShelterDTOs
{
    public abstract class BaseShelterDTO
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
