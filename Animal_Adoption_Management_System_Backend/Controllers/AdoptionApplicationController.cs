using Animal_Adoption_Management_System_Backend.Authorization;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionApplicationDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
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
    public class AdoptionApplicationController : ControllerBase
    {
        private readonly IAdoptionApplicationService _adoptionApplicationService;
        private readonly IMapper _mapper;
        private readonly IPermissionChecker _permissionChecker;

        public AdoptionApplicationController(IMapper mapper, IPermissionChecker permissionChecker, IAdoptionApplicationService adoptionApplicationService)
        {
            _mapper = mapper;
            _permissionChecker = permissionChecker;
            _adoptionApplicationService = adoptionApplicationService;
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<AdoptionApplicationDTO>>> GetAllAdoptionApplications()
        {
            IEnumerable<AdoptionApplication> adoptionApplications = await _adoptionApplicationService.GetAllAsync();
            IEnumerable<AdoptionApplicationDTO> adoptionApplicationDTOs = _mapper.Map<IEnumerable<AdoptionApplicationDTO>>(adoptionApplications);
            return Ok(adoptionApplicationDTOs);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet]
        public async Task<ActionResult<PagedResult<AdoptionApplicationDTO>>> GetPagedAdoptionApplications([FromQuery] QueryParameters queryParameters)
        {
            PagedResult<AdoptionApplicationDTO> pagedResult = await _adoptionApplicationService.GetAllAsync<AdoptionApplicationDTO>(queryParameters);
            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdoptionApplicationDTO>> GetAdoptionApplication(int id)
        {
            AdoptionApplication adoptionApplication = await _adoptionApplicationService.GetAsync(id);

            AdoptionApplicationDTO adoptionApplicationDTO = _mapper.Map<AdoptionApplicationDTO>(adoptionApplication);
            return Ok(adoptionApplicationDTO);
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<AdoptionApplicationDTOWithDetails>> GetAdoptionApplicationWithDetails(int id)
        {
            // Check if User is Applier of this AdApp or Employee at the Shelter of the Animal 
            AdoptionApplication adoptionApplicationWithDetails = await _adoptionApplicationService.GetWithAnimalShelterDetailsAsync(id);
            _permissionChecker.CheckPermissionForAdoptionApplication(adoptionApplicationWithDetails, HttpContext.User);

            AdoptionApplicationDTOWithDetails adoptionApplicationDTOWithDetails = _mapper.Map<AdoptionApplicationDTOWithDetails>(adoptionApplicationWithDetails);
            return Ok(adoptionApplicationDTOWithDetails);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<AdoptionApplicationDTO>>> GetFilteredAdoptionApplications(string? animalName, string? applierName, DateTime? dateAfter, DateTime? dateBefore, ApplicationStatus? status)
        {
            IEnumerable<AdoptionApplication> adoptionApplications = await _adoptionApplicationService.GetFilteredAdoptionApplicationsAsync(animalName, applierName, dateAfter, dateBefore, status);
            IEnumerable<AdoptionApplicationDTOWithDetails> adoptionApplicationDTOs = _mapper.Map<IEnumerable<AdoptionApplicationDTOWithDetails>>(adoptionApplications);
            return Ok(adoptionApplicationDTOs);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("pageAndFilter")]
        public async Task<ActionResult<IEnumerable<AdoptionApplicationDTOWithDetails>>> GetPagedAndFilteredAdoptionApplications([FromQuery] QueryParameters queryParameters, string? animalName, string? applierName, DateTime? dateAfter, DateTime? dateBefore, ApplicationStatus? status)
        {
            PagedResult<AdoptionApplicationDTOWithDetails> adoptionApplicationDTOs = await _adoptionApplicationService.GetPagedAndFilteredAdoptionApplicationsAsync<AdoptionApplicationDTOWithDetails>(queryParameters, animalName, applierName, dateAfter, dateBefore, status);
            return Ok(adoptionApplicationDTOs);
        }

        [HttpPost]
        [Authorize(Policy = "MinimalAge")]
        public async Task<ActionResult<AdoptionApplicationDTO>> CreateAdoptionApplication(CreateAdoptionApplicationDTO applicationDTO)
        {
            AdoptionApplication adoptionApplicationToCreate = _mapper.Map<AdoptionApplication>(applicationDTO);
            AdoptionApplication adoptionApplicationToCreateWithAnimalAndApplier = await _adoptionApplicationService.TryAddAnimalAndApplierToAdoptionApplication(adoptionApplicationToCreate, applicationDTO.AnimalId, applicationDTO.ApplierId);

            AdoptionApplication createdAdoptionApplication = await _adoptionApplicationService.AddAsync(adoptionApplicationToCreateWithAnimalAndApplier);

            AdoptionApplicationDTO createdAdoptionApplicationDTO = _mapper.Map<AdoptionApplicationDTO>(createdAdoptionApplication);
            return CreatedAtAction("GetAdoptionApplication", new { id = createdAdoptionApplication.Id }, createdAdoptionApplicationDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPut("{id}/updateAdoptionApplicationStatus")]
        public async Task<ActionResult<AdoptionApplicationDTO>> UpdateStatus(int id, [FromBody] ApplicationStatus newStatus)
        {
            AdoptionApplication adoptionApplicationWithDetails = await _adoptionApplicationService.GetWithAnimalShelterDetailsAsync(id);
            _permissionChecker.CheckPermissionForAdoptionApplication(adoptionApplicationWithDetails, HttpContext.User);

            AdoptionApplication updatedAdoptionApplication = await _adoptionApplicationService.UpdateAdoptionApplicationStatus(id, newStatus);
            AdoptionApplicationDTO updatedAdoptionApplicationDTO = _mapper.Map<AdoptionApplicationDTO>(updatedAdoptionApplication);
            return Ok(updatedAdoptionApplicationDTO);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdoptionApplication(int id)
        {
            await _adoptionApplicationService.DeleteAsync(id);
            return NoContent();
        }

    }
}
