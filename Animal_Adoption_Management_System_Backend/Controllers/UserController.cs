using Animal_Adoption_Management_System_Backend.Authorization;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionApplicationDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionContractDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.DonationDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ManagedAdoptionContractDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Pagination;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{id}/donation")]
        public async Task<ActionResult<ICollection<DonationDTOWithDetails>>> GetUserDonations(string id)
        {
            _permissionChecker.CheckPermissionForUserRelatedEntity(id, HttpContext.User, "Donation");

            User user = await _userService.GetWithDonationDetailsAsync(id);
            UserDTOWithDetails userDTO = _mapper.Map<UserDTOWithDetails>(user);
            return Ok(userDTO.Donations);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("{id}/image")]
        public async Task<ActionResult<ICollection<ImageDTOWithDetails>>> GetUserImages(string id)
        {
            _permissionChecker.CheckPermissionForUserRelatedEntity(id, HttpContext.User, "Image");

            User user = await _userService.GetWithImageDetailsAsync(id);
            UserDTOWithDetails userDTO = _mapper.Map<UserDTOWithDetails>(user);
            return Ok(userDTO.Images);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("{id}/shelter")]
        public async Task<ActionResult<UserDTOWithDetails>> GetUserShelterInfo(string id)
        {
            _permissionChecker.CheckPermissionForUserRelatedEntity(id, HttpContext.User, "Shelter");

            User user = await _userService.GetWithShelterDetailsAsync(id);
            UserDTOWithDetails userDTO = _mapper.Map<UserDTOWithDetails>(user);
            return Ok(userDTO);
        }

        [HttpGet("{id}/adoptionApplication")]
        public async Task<ActionResult<ICollection<AdoptionApplicationDTOWithDetails>>> GetUserAdoptionApplication(string id)
        {
            _permissionChecker.CheckPermissionForUserRelatedEntity(id, HttpContext.User, "AdoptionApplication");

            User user = await _userService.GetWithAdoptionApplicationDetailsAsync(id);
            UserDTOWithDetails userDTO = _mapper.Map<UserDTOWithDetails>(user);
            return Ok(userDTO.AdoptionApplications);
        }

        [HttpGet("{id}/adoptionContract")]
        public async Task<ActionResult<ICollection<AdoptionContractDTOWithManagerDetails>>> GetUserAdoptionContract(string id)
        {
            _permissionChecker.CheckPermissionForUserRelatedEntity(id, HttpContext.User, "AdoptionContract");

            User user = await _userService.GetWithAdoptionContractDetailsAsync(id);
            UserDTOWithDetails userDTO = _mapper.Map<UserDTOWithDetails>(user);
            return Ok(userDTO.AdoptionsContracts);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("{id}/managedAdoptionContract")]
        public async Task<ActionResult<ICollection<ManagedAdoptionContractDTOWithDetails>>> GetUserManagedAdoptionContract(string id)
        {
            _permissionChecker.CheckPermissionForUserRelatedEntity(id, HttpContext.User, "ManagedAdoptionContract");

            User user = await _userService.GetWithManagedAdoptionContractDetailsAsync(id);
            UserDTOWithDetails userDTO = _mapper.Map<UserDTOWithDetails>(user);
            return Ok(userDTO.ManagedAdoptionsContracts);
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
