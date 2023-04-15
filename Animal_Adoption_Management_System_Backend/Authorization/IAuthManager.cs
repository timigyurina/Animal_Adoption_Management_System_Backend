using Animal_Adoption_Management_System_Backend.Models.DTOs.UserAuthDTOs;
using Microsoft.AspNetCore.Identity;

namespace Animal_Adoption_Management_System_Backend.Authorization
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> RegisterAs(RegisterUserDTO registerUserDTO, string role);
        Task<AuthResponseDTO?> Login(LoginUserDTO loginUserDTO);

        Task<string> CreateRefreshToken();
        Task<AuthResponseDTO?> VerifyRefreshToken(string userId, string token, string refreshToken);
    }
}
