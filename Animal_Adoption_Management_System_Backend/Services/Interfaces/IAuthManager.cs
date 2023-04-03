using Animal_Adoption_Management_System_Backend.Models.DTOs.UserAuthDTOs;
using Microsoft.AspNetCore.Identity;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> RegisterAs(RegisterUserDTO registerUserDTO, string role);
        Task<AuthResponseDTO?> Login(LoginUserDTO loginUserDTO);

        Task<string> CreateRefreshToken();
        Task<AuthResponseDTO?> VerifyRefreshToken(AuthResponseDTO request);
    }
}
