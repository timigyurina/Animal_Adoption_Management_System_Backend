using Animal_Adoption_Management_System_Backend.Models.DTOs.UserAuthDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Animal_Adoption_Management_System_Backend.Authorization
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthManager> _logger;
        private readonly IConfiguration _configuration;

        private User? _user;
        private const string _loginProvider = "AnimalAdoptionManagementSystem";
        private const string _refreshToken = "RefreshToken";

        public AuthManager(IMapper mapper, UserManager<User> userManager, ILogger<AuthManager> logger, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IEnumerable<IdentityError>> RegisterAs(RegisterUserDTO registerUserDTO, string role)
        {
            User user = _mapper.Map<User>(registerUserDTO);
            user.UserName = registerUserDTO.Email;
            user.IsActive = true;
            user.IsContactOfShelter = false;
            user.Shelter = null;

            IdentityResult result = await _userManager.CreateAsync(user, registerUserDTO.Password);

            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, role);

            return result.Errors;
        }

        public async Task<AuthResponseDTO?> Login(LoginUserDTO loginUserDTO)
        {
            _logger.LogInformation($"Looking for user with email {loginUserDTO.Email}");
            //_user = await _userManager.FindByEmailAsync(loginUserDTO.Email);
            _user = await _userManager.Users
                .Include(u => u.Shelter)
                .FirstOrDefaultAsync(u => u.Email == loginUserDTO.Email);
            bool isValidUser = await _userManager.CheckPasswordAsync(_user, loginUserDTO.Password);

            if (_user == null || isValidUser == false)
            {
                _logger.LogWarning($"User with email {loginUserDTO.Email} was not found");
                return null;
            }

            string token = await GenerateToken();
            _logger.LogInformation($"Token generated for user {loginUserDTO.Email}: {token}");

            return new AuthResponseDTO
            {
                UserId = _user.Id,
                Roles = await _userManager.GetRolesAsync(_user),
                Token = token,
                RefreshToken = await CreateRefreshToken()
            };
        }


        public async Task<string> CreateRefreshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user!, _loginProvider, _refreshToken);
            string newRefreshToken = await _userManager.GenerateUserTokenAsync(_user!, _loginProvider, _refreshToken);

            IdentityResult result = await _userManager.SetAuthenticationTokenAsync(_user!, _loginProvider, _refreshToken, newRefreshToken);
            return newRefreshToken;
        }

        public async Task<AuthResponseDTO?> VerifyRefreshToken(AuthResponseDTO request)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);

            // search for the username with this token
            string? userName = tokenContent.Claims.ToList()
                .FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)?.Value;
            _user = await _userManager.FindByNameAsync(userName);
            if (_user == null || _user.Id != request.UserId)
                return null;

            bool isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, request.RefreshToken);

            if (isValidRefreshToken)
            {
                string token = await GenerateToken();
                return new AuthResponseDTO
                {
                    UserId = _user.Id,
                    Token = token,
                    Roles = await _userManager.GetRolesAsync(_user),
                    RefreshToken = await CreateRefreshToken()
                };
            }
            // if the token was not valid, this signs out any saved login for the oser
            await _userManager.UpdateSecurityStampAsync(_user);
            return null;
        }

        private async Task<string> GenerateToken()
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
            SigningCredentials credenials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            IEnumerable<string> roles = await _userManager.GetRolesAsync(_user!);

            IEnumerable<Claim> roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            //IEnumerable<Claim> userClaims = await _userManager.GetClaimsAsync(user);

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user!.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim(ClaimTypes.DateOfBirth, _user.DateOfBirth.ToString()),
                new Claim("UserId", _user.Id),
            }
            .Union(roleClaims).ToList();

            if (_user.Shelter != null)
            {
                Claim shelterClaim = new Claim("ShelterId", _user.Shelter.Id.ToString());
                claims.Add(shelterClaim);
            }

            JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                    signingCredentials: credenials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
