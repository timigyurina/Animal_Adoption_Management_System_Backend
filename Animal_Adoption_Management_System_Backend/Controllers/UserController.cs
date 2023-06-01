using Animal_Adoption_Management_System_Backend.Authorization;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionApplicationDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionContractDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalShelterDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.DonationDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ManagedAdoptionContractDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ShelterDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Models.Pagination;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Animal_Adoption_Management_System_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IShelterService _shelterService;
        private readonly IMapper _mapper;
        private readonly IPermissionChecker _permissionChecker;

        public UserController(IMapper mapper, IPermissionChecker permissionChecker, IUserService userService, IShelterService shelterService)
        {
            _mapper = mapper;
            _permissionChecker = permissionChecker;
            _userService = userService;
            _shelterService = shelterService;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            IEnumerable<User> users = await _userService.GetAllAsync();
            IEnumerable<UserDTO> userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);
            return Ok(userDTOs);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<ActionResult<PagedResult<UserDTO>>> GetPagedUsers([FromQuery] QueryParameters queryParameters)
        {
            PagedResult<UserDTO> pagedResult = await _userService.GetAllAsync<UserDTO>(queryParameters);
            return Ok(pagedResult);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("{email}")]
        public async Task<ActionResult<UserDTO>> GetUserByEmail(string email)
        {
            User user = await _userService.GetByEmailAsync(email);

            UserDTO userDTO = _mapper.Map<UserDTO>(user);
            return Ok(userDTO);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("{id}/details")]
        public async Task<ActionResult<UserDTOWithDetails>> GetUserWithDetails(string id)
        {
            User user = await _userService.GetWithAllDetailsAsync(id);

            UserDTOWithDetails userDTO = _mapper.Map<UserDTOWithDetails>(user);
            return Ok(userDTO);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<UserDTOWithDetails>>> GetFilteredUsers(string? name, string? email, bool? isActive, bool? isContactOfShelter, string? shelterName, DateTime? bornAfter, DateTime? bornBefore)
        {
            IEnumerable<User> users = await _userService.GetFilteredUsersAsync(name, email, isActive, isContactOfShelter, shelterName, bornAfter, bornBefore);
            IEnumerable<UserDTOWithDetails> userDTOs = _mapper.Map<IEnumerable<UserDTOWithDetails>>(users);
            return Ok(userDTOs);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("pageAndFilter")]
        public async Task<ActionResult<IEnumerable<UserDTOWithDetails>>> GetPagedAndFilteredUsers([FromQuery] QueryParameters queryParameters, string? name, string? email, bool? isActive, bool? isContactOfShelter, string? shelterName, DateTime? bornAfter, DateTime? bornBefore)
        {
            PagedResult<UserDTOWithDetails> userDTOs = await _userService.GetPagedAndFilteredUsersAsync<UserDTOWithDetails>(queryParameters, name, email, isActive, isContactOfShelter, shelterName, bornAfter, bornBefore);
            return Ok(userDTOs);
        }

        // All User has these entities
        [HttpGet("donation")]
        public async Task<ActionResult<ICollection<DonationDTOWithDetails>>> GetUserDonations()
        {
            string? userIdClaimValue = Request.Cookies.FirstOrDefault(x => x.Key == "X-UserId").Value;
            if (userIdClaimValue == null)
                return Unauthorized();

            User user = await _userService.GetWithDonationDetailsAsync(userIdClaimValue);
            UserDTOWithDetails userDTO = _mapper.Map<UserDTOWithDetails>(user);
            return Ok(userDTO.Donations);
        }

        [HttpGet("adoptionApplication")]
        public async Task<ActionResult<ICollection<AdoptionApplicationDTOWithDetails>>> GetUserAdoptionApplications()
        {
            string? userIdClaimValue = Request.Cookies.FirstOrDefault(x => x.Key == "X-UserId").Value;
            if (userIdClaimValue == null)
                return Unauthorized();

            User user = await _userService.GetWithAdoptionApplicationDetailsAsync(userIdClaimValue);
            IEnumerable<AdoptionApplicationDTOWithDetails> adoptionApplicationDTOs = _mapper.Map<IEnumerable<AdoptionApplicationDTOWithDetails>>(user.AdoptionApplications);
            return Ok(adoptionApplicationDTOs);
        }

        [HttpGet("adoptionContract")]
        public async Task<ActionResult<ICollection<AdoptionContractDTOWithManagerDetails>>> GetUserAdoptionContracts()
        {
            string? userIdClaimValue = Request.Cookies.FirstOrDefault(x => x.Key == "X-UserId").Value;
            if (userIdClaimValue == null)
                return Unauthorized();

            User user = await _userService.GetWithAdoptionContractDetailsAsync(userIdClaimValue);
            IEnumerable<AdoptionContractDTOWithDetails> adoptionContractDTOs = _mapper.Map<IEnumerable<AdoptionContractDTOWithDetails>>(user.AdoptionsContracts);
            return Ok(adoptionContractDTOs);
        }

        // Only ShelterEmployee has these entites (they belong to the Sheter of the employee))
        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("shelter")]
        public async Task<ActionResult<UserDTOWithDetails>> GetUserShelterInfo()
        {
            Claim? shelterIdClaim = User.Claims.FirstOrDefault(x => x.Type == "ShelterId") ?? throw new BadRequestException("User is not an Employee of any Shelter");

            Shelter shelterWithDetails = await _shelterService.GetWithDetailsAsync(int.Parse(shelterIdClaim.Value));
            ShelterDTOWithDetails shelterDTOWithDetails = _mapper.Map<ShelterDTOWithDetails>(shelterWithDetails);
            return Ok(shelterDTOWithDetails);
        }
        
        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("shelter/animal")]
        public async Task<ActionResult<UserDTOWithDetails>> GetUserShelterAnimals()
        {
            Claim? shelterIdClaim = User.Claims.FirstOrDefault(x => x.Type == "ShelterId") ?? throw new BadRequestException("User is not an Employee of any Shelter");

            Shelter shelterWithAnimals = await _shelterService.GetWithAddressAndAnimalsAsync(int.Parse(shelterIdClaim.Value));
            IEnumerable<AnimalShelterDTO> animals = _mapper.Map<IEnumerable<AnimalShelterDTO>>(shelterWithAnimals.Animals);
            return Ok(animals);
        }
        
        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("shelter/donation")]
        public async Task<ActionResult<UserDTOWithDetails>> GetUserShelterDonations()
        {
            Claim? shelterIdClaim = User.Claims.FirstOrDefault(x => x.Type == "ShelterId") ?? throw new BadRequestException("User is not an Employee of any Shelter");

            Shelter shelterWithDonations = await _shelterService.GetWithDonationsAsync(int.Parse(shelterIdClaim.Value));
            IEnumerable<DonationDTOWithDetails> donations = _mapper.Map<IEnumerable<DonationDTOWithDetails>>(shelterWithDonations.Donations);
            return Ok(donations);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("image")]
        public async Task<ActionResult<ICollection<ImageDTOWithDetails>>> GetImagesUploadedByUser()
        {
            string? userIdClaimValue = Request.Cookies.FirstOrDefault(x => x.Key == "X-UserId").Value;
            if (userIdClaimValue == null)
                return Unauthorized();

            User user = await _userService.GetWithImageDetailsAsync(userIdClaimValue);
            IEnumerable<ImageDTOWithAnimal> imageDTOs = _mapper.Map<IEnumerable<ImageDTOWithAnimal>>(user.Images);
            return Ok(imageDTOs);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("managedAdoptionContract")]
        public async Task<ActionResult<ICollection<ManagedAdoptionContractDTOWithDetails>>> GetAdoptionContractsManagedByUser()
        {
            string? userIdClaimValue = Request.Cookies.FirstOrDefault(x => x.Key == "X-UserId").Value;
            if (userIdClaimValue == null)
                return Unauthorized();

            User user = await _userService.GetWithManagedAdoptionContractDetailsAsync(userIdClaimValue);
            IEnumerable<ManagedAdoptionContractDTOWithDetails> managedAdoptionContractDTOs = _mapper.Map<IEnumerable<ManagedAdoptionContractDTOWithDetails>>(user.ManagedAdoptionsContracts);
            return Ok(managedAdoptionContractDTOs);
        }


        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}/updateUserIsActive")]
        public async Task<ActionResult<UserDTO>> UpdateStatus(string id, [FromBody] bool isActive)
        {
            User updatedUser = await _userService.UpdateUserIsActive(id, isActive);
            UserDTO updatedUserDTO = _mapper.Map<UserDTO>(updatedUser);
            return Ok(updatedUserDTO);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}/updateConnectionWithShelter")]
        public async Task<ActionResult<UserDTOWithDetails>> UpdateConnectionWithShelter(string id, UpdateUserConnectionWithShelterDTO updateUserConnectionWithShelterDTO)
        {
            Shelter? shelter = null;
            if (updateUserConnectionWithShelterDTO.ShelterId != null)
                shelter = await _shelterService.GetAsync((int)updateUserConnectionWithShelterDTO.ShelterId);

            User updatedUser = await _userService.UpdateConnectionWithShelterById(shelter, id, updateUserConnectionWithShelterDTO.IsContactOfShelter);
            UserDTOWithDetails updatedUserDTOWithDetails = _mapper.Map<UserDTOWithDetails>(updatedUser);
            return Ok(updatedUserDTOWithDetails);
        }
    }
}
