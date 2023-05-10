using Animal_Adoption_Management_System_Backend.Authorization;
using Animal_Adoption_Management_System_Backend.Models.DTOs.DonationDTOs;
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
    public class DonationController : ControllerBase
    {
        private readonly IDonationService _donationService;
        private readonly IMapper _mapper;
        private readonly IPermissionChecker _permissionChecker;

        public DonationController(IMapper mapper, IPermissionChecker permissionChecker, IDonationService donationService)
        {
            _mapper = mapper;
            _permissionChecker = permissionChecker;
            _donationService = donationService;
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<DonationDTO>>> GetAllDonations()
        {
            IEnumerable<Donation> donations = await _donationService.GetAllAsync();
            IEnumerable<DonationDTO> donationDTOs = _mapper.Map<IEnumerable<DonationDTO>>(donations);
            return Ok(donationDTOs);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet]
        public async Task<ActionResult<PagedResult<DonationDTO>>> GetPagedDonations([FromQuery] QueryParameters queryParameters)
        {
            PagedResult<DonationDTO> pagedResult = await _donationService.GetAllAsync<DonationDTO>(queryParameters);
            return Ok(pagedResult);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("{id}")]
        public async Task<ActionResult<DonationDTO>> GetDonation(int id)
        {
            Donation donation = await _donationService.GetAsync(id);

            DonationDTO donationDTO = _mapper.Map<DonationDTO>(donation);
            return Ok(donationDTO);
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<DonationDTOWithDetails>> GetDonationWithDetails(int id)
        {
            Donation donationWithDetails = await _donationService.GetWithDetailsAsync(id);
            _permissionChecker.CheckPermissionForDonation(donationWithDetails, HttpContext.User);

            DonationDTOWithDetails donationDTOWithDetails = _mapper.Map<DonationDTOWithDetails>(donationWithDetails);
            return Ok(donationDTOWithDetails);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<DonationDTOWithDetails>>> GetFilteredDonations(string? shelterName, string? donatorName, decimal? minAmount, decimal? maxAmount, DateTime? dateAfter, DateTime? dateBefore, DonationStatus? status)
        {
            IEnumerable<Donation> donations = await _donationService.GetFilteredDonationsAsync(shelterName, donatorName, minAmount, maxAmount, dateAfter, dateBefore, status);
            IEnumerable<DonationDTOWithDetails> donationDTOs = _mapper.Map<IEnumerable<DonationDTOWithDetails>>(donations);
            return Ok(donationDTOs);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("pageAndFilter")]
        public async Task<ActionResult<IEnumerable<DonationDTOWithDetails>>> GetPagedAndFilteredDonations([FromQuery] QueryParameters queryParameters, string? shelterName, string? donatorName, decimal? minAmount, decimal? maxAmount, DateTime? dateAfter, DateTime? dateBefore, DonationStatus? status)
        {
            PagedResult<DonationDTOWithDetails> donationDTOs = await _donationService.GetPagedAndFilteredDonationsAsync<DonationDTOWithDetails>(queryParameters, shelterName, donatorName, minAmount, maxAmount, dateAfter, dateBefore, status);
            return Ok(donationDTOs);
        }

        [Authorize(Policy = "MinimalAge")]
        [HttpPost]
        public async Task<ActionResult<DonationDTO>> CreateDonation(CreateDonationDTO donationDTO)
        {
            Donation donationToCreate = _mapper.Map<Donation>(donationDTO);
            Donation donationToCreateWithDonatorAndShelter = await _donationService.TryAddDonatorAndShelterToDonation(donationToCreate, donationDTO.UserId, donationDTO.ShelterId);

            Donation createdDonation = await _donationService.AddAsync(donationToCreateWithDonatorAndShelter);

            DonationDTO createdDonationDTO = _mapper.Map<DonationDTO>(createdDonation);
            return CreatedAtAction("GetDonation", new { id = createdDonation.Id }, createdDonationDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPut("{id}/updateDonationStatus")]
        public async Task<ActionResult<DonationDTO>> UpdateStatus(int id, [FromBody] DonationStatus newStatus)
        {
            Donation donationWithDetails = await _donationService.GetWithDetailsAsync(id);
            _permissionChecker.CheckPermissionForDonation(donationWithDetails, HttpContext.User);

            Donation updatedDonation = await _donationService.UpdateDonationStatus(id, newStatus);

            DonationDTO updatedDonationDTO = _mapper.Map<DonationDTO>(updatedDonation);
            return Ok(updatedDonationDTO);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonation(int id)
        {
            await _donationService.DeleteAsync(id);
            return NoContent();
        }
    }
}
