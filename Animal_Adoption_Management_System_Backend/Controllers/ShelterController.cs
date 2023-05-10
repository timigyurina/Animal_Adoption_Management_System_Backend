using Animal_Adoption_Management_System_Backend.Authorization;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ShelterDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Models.Pagination;
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
        private readonly IShelterService _shelterService;
        private readonly IMapper _mapper;
        private readonly IPermissionChecker _permissionChecker;

        public ShelterController(IMapper mapper, IPermissionChecker permissionChecker, IShelterService shelterService)
        {
            _mapper = mapper;
            _permissionChecker = permissionChecker;
            _shelterService = shelterService;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<ShelterDTO>>> GetAllShelters()
        {
            IEnumerable<Shelter> shelters = await _shelterService.GetAllAsync();
            IEnumerable<ShelterDTO> shelterDTOs = _mapper.Map<IEnumerable<ShelterDTO>>(shelters);
            return Ok(shelterDTOs);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ShelterDTO>>> GetPagedShelters([FromQuery] QueryParameters queryParameters)
        {
            PagedResult<ShelterDTO> pagedResult = await _shelterService.GetAllAsync<ShelterDTO>(queryParameters);
            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShelterDTO>> GetShelter(int id)
        {
            Shelter shelter = await _shelterService.GetAsync(id);

            ShelterDTO shelterDTO = _mapper.Map<ShelterDTO>(shelter);
            return Ok(shelterDTO);
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<ShelterDTOWithDetails>> GetShelterWithDetails(int id)
        {
            Shelter shelterWithDetails = await _shelterService.GetWithDetailsAsync(id);
            _permissionChecker.CheckPermissionForShelter(id, HttpContext.User);
            ShelterDTOWithDetails shelterDTOWithDetails = _mapper.Map<ShelterDTOWithDetails>(shelterWithDetails);
            return Ok(shelterDTOWithDetails);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ShelterDTOWithDetails>>> GetFilteredShelters(string? name, string? contactPersonName, bool? isActive)
        {
            IEnumerable<Shelter> shelters = await _shelterService.GetFilteredSheltersAsync(name, contactPersonName, isActive);
            IEnumerable<ShelterDTOWithDetails> shelterDTOs = _mapper.Map<IEnumerable<ShelterDTOWithDetails>>(shelters);
            return Ok(shelterDTOs);
        }

        [HttpGet("pageAndFilter")]
        public async Task<ActionResult<IEnumerable<ShelterDTOWithDetails>>> GetPagedAndFilteredShelters([FromQuery] QueryParameters queryParameters, string? name, string? contactPersonName, bool? isActive)
        {
            PagedResult<ShelterDTOWithDetails> shelterDTOs = await _shelterService.GetPagedAndFilteredSheltersAsync<ShelterDTOWithDetails>(queryParameters, name, contactPersonName, isActive);
            return Ok(shelterDTOs);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult<ShelterDTO>> CreateShelter(CreateShelterDTO shelterDTO)
        {
            Shelter shelterToCreate = _mapper.Map<Shelter>(shelterDTO);

            Shelter createdShelter = await _shelterService.AddAsync(shelterToCreate);

            ShelterDTO createdShelterDTO = _mapper.Map<ShelterDTO>(createdShelter);
            return CreatedAtAction("GetShelter", new { id = createdShelter.Id }, createdShelterDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPut("{id}/updateShelterContactInfo")]
        public async Task<ActionResult<ShelterDTO>> UpdateShelterContactInfo(int id, UpdateShelterContactInfoDTO shelterDTO)
        {
            Shelter shelterToUpdate = await _shelterService.GetWithAddressAsync(id);
            _permissionChecker.CheckPermissionForShelter(id, HttpContext.User);

            _mapper.Map(shelterDTO, shelterToUpdate);

            try
            {
                await _shelterService.UpdateAsync(shelterToUpdate);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _shelterService.Exists(id))
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
            Shelter updatedShelter = await _shelterService.UpdateShelterIsActive(id, isActive);
            _permissionChecker.CheckPermissionForShelter(id, HttpContext.User);

            ShelterDTO updatedShelterDTO = _mapper.Map<ShelterDTO>(updatedShelter);
            return Ok(updatedShelterDTO);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShelter(int id)
        {
            await _shelterService.DeleteAsync(id);
            return NoContent();
        }
    }
}
