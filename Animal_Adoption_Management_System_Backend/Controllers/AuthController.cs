using Animal_Adoption_Management_System_Backend.Authorization;
using Animal_Adoption_Management_System_Backend.Models.DTOs.UserAuthDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
                if (response == null)
                    return Unauthorized();

                CookieOptions options = new() { HttpOnly = true, Secure = true, Path = "/", Expires = DateTime.Now.AddDays(1), SameSite = SameSiteMode.None };
                Response.Cookies.Append("X-Access-Token", response.Token, options);
                Response.Cookies.Append("X-Refresh-Token", response.RefreshToken, options);
                Response.Cookies.Append("X-UserId", response.UserId, options);
                Response.Cookies.Append("X-UserRoles", string.Join(',', response.Roles), new CookieOptions()
                {
                    HttpOnly = false,
                    Secure = true,
                    Path = "/",
                    Expires = DateTime.Now.AddDays(1),
                    SameSite = SameSiteMode.None
                });

                _logger.LogInformation($"User {loginDTO.Email} has logged in successfully at {DateTime.Now}");
                return Ok();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, $"Something went wrong in the {nameof(Login)}");
                return Problem($"Something went wrong in the {nameof(Login)}. Please contact support!", statusCode: 500);
            }
        }
        [HttpGet("logout")]
        public ActionResult Logout()
        {
            //Response.Cookies.Delete("X-Access-Token");
            CookieOptions options = new() { HttpOnly = true, Secure = true, Path = "/", Expires = DateTime.Now.AddDays(-1), SameSite = SameSiteMode.None };
            Response.Cookies.Append("X-Access-Token", "", options);
            Response.Cookies.Append("X-Refresh-Token", "", options);
            Response.Cookies.Append("X-UserId", "", options);
            Response.Cookies.Append("X-UserRoles", "", options);
            return Ok();
        }

        [Authorize]
        [HttpGet("validateUser")]
        public ActionResult ValidateUser()
        {
            return Ok();
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

        [Authorize(Roles = "Administrator")]
        [HttpPost]
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

        [Authorize(Roles = "Administrator")]
        [HttpPost]
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

        [HttpGet("refreshToken")]
        public async Task<IActionResult> Refresh()
        {
            if (!(Request.Cookies.TryGetValue("X-UserId", out var userId) && Request.Cookies.TryGetValue("X-Access-Token", out var token)
                && Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken)) || userId == null || token == null || refreshToken == null)
                return BadRequest();

            AuthResponseDTO? authResponse = await _authManager.VerifyRefreshToken(userId, token, refreshToken);
            if (authResponse == null)
                return Unauthorized();

            CookieOptions options = new() { HttpOnly = true, Secure = true, Path = "/", Expires = DateTime.Now.AddDays(1), SameSite = SameSiteMode.None };
            Response.Cookies.Append("X-Access-Token", authResponse.Token, options);
            Response.Cookies.Append("X-Refresh-Token", authResponse.RefreshToken, options);
            Response.Cookies.Append("X-UserId", authResponse.UserId, options);
            Response.Cookies.Append("X-UserRoles", string.Join(',', authResponse.Roles), new CookieOptions()
            {
                HttpOnly = false,
                Secure = true,
                Path = "/",
                Expires = DateTime.Now.AddDays(1),
                SameSite = SameSiteMode.None
            });

            return Ok();
        }

        //[HttpPost]
        //[Route("refreshToken")]
        //public async Task<ActionResult> RefreshToken(RefreshTokenRequestDTO request)
        //{
        //    AuthResponseDTO? authResponse = await _authManager.VerifyRefreshToken(request.UserId, request.Token, request.RefreshToken);

        //    if (authResponse == null)
        //        return Unauthorized();

        //    return Ok(authResponse);
        //}

    }
}
