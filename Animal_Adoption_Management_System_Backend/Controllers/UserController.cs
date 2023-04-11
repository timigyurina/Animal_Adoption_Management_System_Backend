using Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionApplicationDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionContractDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.DonationDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ManagedAdoptionContractDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Animal_Adoption_Management_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            IEnumerable<User> users = await _unitOfWork.UserService.GetAllAsync();
            IEnumerable<UserDTO> userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);
            return Ok(userDTOs);
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<UserDTO>> GetUserByEmail(string email)
        {
            User user = await _unitOfWork.UserService.GetByEmailAsync(email);

            UserDTO userDTO = _mapper.Map<UserDTO>(user);
            return Ok(userDTO);
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<UserDTOWithDetails>> GetUserWithDetails(string id)
        {
            User user = await _unitOfWork.UserService.GetWithAllDetailsAsync(id);

            UserDTOWithDetails userDTO = _mapper.Map<UserDTOWithDetails>(user);
            return Ok(userDTO);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<UserDTOWithDetails>>> GetFilteredUsers(string? name, string? email, bool? isActive, bool? isContactOfShelter, string? shelterName, DateTime? bornAfter, DateTime? bornBefore)
        {
            IEnumerable<User> users = await _unitOfWork.UserService.GetFilteredUsersAsync(name, email, isActive, isContactOfShelter, shelterName, bornAfter, bornBefore);
            IEnumerable<UserDTOWithDetails> userDTOs = _mapper.Map<IEnumerable<UserDTOWithDetails>>(users);
            return Ok(userDTOs);
        }

        [HttpGet("{id}/donation")]
        public async Task<ActionResult<ICollection<DonationDTOWithDetails>>> GetUserDonations(string id)
        {
            User user = await _unitOfWork.UserService.GetWithDonationDetailsAsync(id);
            UserDTOWithDetails userDTO = _mapper.Map<UserDTOWithDetails>(user);
            return Ok(userDTO.Donations);
        }

        [HttpGet("{id}/image")]
        public async Task<ActionResult<ICollection<ImageDTOWithDetails>>> GetUserImages(string id)
        {
            User user = await _unitOfWork.UserService.GetWithImageDetailsAsync(id);
            UserDTOWithDetails userDTO = _mapper.Map<UserDTOWithDetails>(user);
            return Ok(userDTO.Images);
        }

        [HttpGet("{id}/shelter")]
        public async Task<ActionResult<UserDTOWithDetails>> GetUserShelterInfo(string id)
        {
            User user = await _unitOfWork.UserService.GetWithShelterDetailsAsync(id);
            UserDTOWithDetails userDTO = _mapper.Map<UserDTOWithDetails>(user);
            return Ok(userDTO);
        }

        [HttpGet("{id}/adoptionApplication")]
        public async Task<ActionResult<ICollection<AdoptionApplicationDTOWithDetails>>> GetUserAdoptionApplication(string id)
        {
            User user = await _unitOfWork.UserService.GetWithAdoptionApplicationDetailsAsync(id);
            UserDTOWithDetails userDTO = _mapper.Map<UserDTOWithDetails>(user);
            return Ok(userDTO.AdoptionApplications);
        }

        [HttpGet("{id}/adoptionContract")]
        public async Task<ActionResult<ICollection<AdoptionContractDTOWithManagerDetails>>> GetUserAdoptionContract(string id)
        {
            User user = await _unitOfWork.UserService.GetWithAdoptionContractDetailsAsync(id);
            UserDTOWithDetails userDTO = _mapper.Map<UserDTOWithDetails>(user);
            return Ok(userDTO.AdoptionsContracts);
        }

        [HttpGet("{id}/managedAdoptionContract")]
        public async Task<ActionResult<ICollection<ManagedAdoptionContractDTOWithDetails>>> GetUserManagedAdoptionContract(string id)
        {
            User user = await _unitOfWork.UserService.GetWithManagedAdoptionContractDetailsAsync(id);
            UserDTOWithDetails userDTO = _mapper.Map<UserDTOWithDetails>(user);
            return Ok(userDTO.ManagedAdoptionsContracts);
        }

        [HttpPut("{id}/updateUserIsActive")]
        public async Task<ActionResult<UserDTO>> UpdateStatus(string id, [FromBody] bool isActive)
        {
            User updatedUser = await _unitOfWork.UserService.UpdateUserIsActive(id, isActive);
            UserDTO updatedUserDTO = _mapper.Map<UserDTO>(updatedUser);
            return Ok(updatedUserDTO);
        }

        [HttpPut("{id}/updateConnectionWithShelter")]
        public async Task<ActionResult<UserDTOWithDetails>> UpdateConnectionWithShelter(string id, UpdateUserConnectionWithShelterDTO updateUserConnectionWithShelterDTO)
        {
            Shelter? shelter = null;
            if (updateUserConnectionWithShelterDTO.ShelterId != null)
                shelter = await _unitOfWork.ShelterService.GetAsync((int)updateUserConnectionWithShelterDTO.ShelterId);

            User updatedUser = await _unitOfWork.UserService.UpdateConnectionWithShelterById(shelter, id, updateUserConnectionWithShelterDTO.IsContactOfShelter);
            UserDTOWithDetails updatedUserDTOWithDetails = _mapper.Map<UserDTOWithDetails>(updatedUser);
            return Ok(updatedUserDTOWithDetails);
        }
    }
}
