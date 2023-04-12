using Animal_Adoption_Management_System_Backend.Authorization;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ShelterDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Animal_Adoption_Management_System_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShelterController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPermissionChecker _permissionChecker;

        public ShelterController(IUnitOfWork unitOfWork, IMapper mapper, IPermissionChecker permissionChecker)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _permissionChecker = permissionChecker;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShelterDTO>>> GetAllShelters()
        {
            IEnumerable<Shelter> shelters = await _unitOfWork.ShelterService.GetAllAsync();
            IEnumerable<ShelterDTO> shelterDTOs = _mapper.Map<IEnumerable<ShelterDTO>>(shelters);
            return Ok(shelterDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShelterDTO>> GetShelter(int id)
        {
            Shelter shelter = await _unitOfWork.ShelterService.GetAsync(id);

            ShelterDTO shelterDTO = _mapper.Map<ShelterDTO>(shelter);
            return Ok(shelterDTO);
        }

        [HttpGet("{id}/details")]
        //[ShelterIdAuthorize("{id}")]
        public async Task<ActionResult<ShelterDTOWithDetails>> GetShelterWithDetails(int id)
        {
            _permissionChecker.CheckPermissionForShelter(id, HttpContext.User);
            Shelter shelterWithDetails = await _unitOfWork.ShelterService.GetWithDetailsAsync(id);
            ShelterDTOWithDetails shelterDTOWithDetails = _mapper.Map<ShelterDTOWithDetails>(shelterWithDetails);
            return Ok(shelterDTOWithDetails);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ShelterDTOWithDetails>>> GetFilteredShelters(string? name, string? contactPersonName, bool? isActive)
        {
            IEnumerable<Shelter> shelters = await _unitOfWork.ShelterService.GetFilteredSheltersAsync(name, contactPersonName, isActive);
            IEnumerable<ShelterDTOWithDetails> shelterDTOs = _mapper.Map<IEnumerable<ShelterDTOWithDetails>>(shelters);
            return Ok(shelterDTOs);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult<ShelterDTO>> CreateShelter(CreateShelterDTO shelterDTO)
        {
            Shelter shelterToCreate = _mapper.Map<Shelter>(shelterDTO);

            Shelter createdShelter = await _unitOfWork.ShelterService.AddAsync(shelterToCreate);

            ShelterDTO createdShelterDTO = _mapper.Map<ShelterDTO>(createdShelter);
            return CreatedAtAction("GetShelter", new { id = createdShelter.Id }, createdShelterDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPut("{id}/updateShelterContactInfo")]
        public async Task<ActionResult<ShelterDTO>> UpdateShelterContactInfo(int id, UpdateShelterContactInfoDTO shelterDTO)
        {
            _permissionChecker.CheckPermissionForShelter(id, HttpContext.User); 
            Shelter shelterToUpdate = await _unitOfWork.ShelterService.GetWithAddressAsync(id);

            _mapper.Map(shelterDTO, shelterToUpdate);

            try
            {
                await _unitOfWork.ShelterService.UpdateAsync(shelterToUpdate);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _unitOfWork.ShelterService.Exists(id))
                    throw new NotFoundException(typeof(Shelter).Name, id);
                else
                    throw;
            }

            ShelterDTOWithDetails updatedShelter = _mapper.Map<ShelterDTOWithDetails>(shelterToUpdate);
            return Ok(updatedShelter);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}/updateShelterIsActive")]
        public async Task<ActionResult<ShelterDTO>> UpdateStatus(int id, [FromBody] bool isActive)
        {
            _permissionChecker.CheckPermissionForShelter(id, HttpContext.User);

            Shelter updatedShelter = await _unitOfWork.ShelterService.UpdateShelterIsActive(id, isActive);

            ShelterDTO updatedShelterDTO = _mapper.Map<ShelterDTO>(updatedShelter);
            return Ok(updatedShelterDTO);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShelter(int id)
        {
            await _unitOfWork.ShelterService.DeleteAsync(id);
            return NoContent();
        }
    }
}
