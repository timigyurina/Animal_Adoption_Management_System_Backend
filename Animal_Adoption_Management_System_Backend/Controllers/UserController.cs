using Animal_Adoption_Management_System_Backend.Models.DTOs.ShelterDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.UserAuthDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Services.Implementations;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
