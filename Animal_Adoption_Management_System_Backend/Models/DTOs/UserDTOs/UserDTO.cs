namespace Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs
{
    public class UserDTO : BaseUserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
    }
}
