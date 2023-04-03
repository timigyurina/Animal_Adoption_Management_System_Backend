using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalShelterDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.UserAuthDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Animal_Adoption_Management_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly ILogger<AuthController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IAuthManager authManager, ILogger<AuthController> logger, IUnitOfWork unitOfWork)
        {
            _authManager = authManager;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginUserDTO loginDTO)
        {
            _logger.LogInformation($"Login attempt for {loginDTO.Email}");

            try
            {
                AuthResponseDTO? response = await _authManager.Login(loginDTO);
                //Response.Cookies.Append("X-Access-Token", response.Token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

                if (response == null)
                    return Unauthorized();

                _logger.LogInformation($"User {loginDTO.Email} has logged in successfully at {DateTime.Now}");
                return Ok(response);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, $"Something went wrong in the {nameof(Login)}");
                return Problem($"Something went wrong in the {nameof(Login)}. Please contact support!", statusCode: 500);
            }

        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register(RegisterUserDTO registerUserDTO)
        {
            _logger.LogInformation($"Registration attempt for {registerUserDTO.Email}");
            try
            {
                IEnumerable<IdentityError> errors = await _authManager.RegisterAs(registerUserDTO, "Adopter");
                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                        Console.WriteLine(error.Code);
                    }
                    return BadRequest(ModelState);
                }

                _logger.LogInformation($"User {registerUserDTO.Email} has registered successfully at {DateTime.Now}");
                return Ok();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, $"Something went wrong in the {nameof(Register)} - user registration attempt for {registerUserDTO.Email}");
                return Problem($"Something went wrong in the {nameof(Register)}, please contact support", statusCode: 500);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [Route("registerAdmin")]
        public async Task<ActionResult> RegisterAdmin(RegisterUserDTO registerUserDTO)
        {
            _logger.LogInformation($"Admin registration attempt for {registerUserDTO.Email}");

            try
            {
                IEnumerable<IdentityError> errors = await _authManager.RegisterAs(registerUserDTO, "Administrator");
                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                        Console.WriteLine(error.Code);
                    }

                    return BadRequest(ModelState);
                }

                _logger.LogInformation($"User {registerUserDTO.Email} has been registered successfully as an Administrator at {DateTime.Now}");
                return Ok();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, $"Something went wrong in the {nameof(RegisterAdmin)}");
                return Problem($"Something went wrong in the {nameof(RegisterAdmin)}. Please contact support!", statusCode: 500);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [Route("registerEmployee")]
        public async Task<ActionResult> RegisterEmployee(RegisterShelterEmployeeDTO registerUserDTO)
        {
            _logger.LogInformation($"Employee registration attempt for {registerUserDTO.Email}");
            Shelter shelter = await _unitOfWork.ShelterService.GetAsync(registerUserDTO.ShelterId);
            try
            {
                IEnumerable<IdentityError> errors = await _authManager.RegisterAs(registerUserDTO, "ShelterEmployee");
                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                        Console.WriteLine(error.Code);
                    }

                    return BadRequest(ModelState);
                }

                // Add User to Shelter as employee
                await _unitOfWork.UserService.CreateConnectionWithShelterByEmail(shelter, registerUserDTO.Email, registerUserDTO.IsContactOfShelter);

                _logger.LogInformation($"User {registerUserDTO.Email} has been registered successfully as a ShelterEmployee at {DateTime.Now}");
                return Ok();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, $"Something went wrong in the {nameof(RegisterEmployee)}");
                return Problem($"Something went wrong in the {nameof(RegisterEmployee)}. Please contact support!", statusCode: 500);
            }
        }


        [HttpPost]
        [Route("refreshToken")]
        public async Task<ActionResult> RefreshToken(AuthResponseDTO request)
        {
            AuthResponseDTO? authResponse = await _authManager.VerifyRefreshToken(request);

            if (authResponse == null)
                return Unauthorized();

            return Ok(authResponse);
        }
    }
}
