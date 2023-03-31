namespace Animal_Adoption_Management_System_Backend.Models.DTOs.UserAuthDTOs
{
    public class AuthResponseDTO
    {
        public string UserId { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
