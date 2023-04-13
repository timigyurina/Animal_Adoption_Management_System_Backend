using Animal_Adoption_Management_System_Backend.Authorization;
using Animal_Adoption_Management_System_Backend.Models.DTOs.DonationDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPermissionChecker _permissionChecker;

        public DonationController(IUnitOfWork unitOfWork, IMapper mapper, IPermissionChecker permissionChecker)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _permissionChecker = permissionChecker;
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonationDTO>>> GetAllDonations()
        {
            IEnumerable<Donation> donations = await _unitOfWork.DonationService.GetAllAsync();
            IEnumerable<DonationDTO> donationDTOs = _mapper.Map<IEnumerable<DonationDTO>>(donations);
            return Ok(donationDTOs);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("{id}")]
        public async Task<ActionResult<DonationDTO>> GetDonation(int id)
        {
            Donation donation = await _unitOfWork.DonationService.GetAsync(id);

            DonationDTO donationDTO = _mapper.Map<DonationDTO>(donation);
            return Ok(donationDTO);
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<DonationDTOWithDetails>> GetDonationWithDetails(int id)
        {
            Donation donationWithDetails = await _unitOfWork.DonationService.GetWithDetailsAsync(id);
            _permissionChecker.CheckPermissionForDonation(donationWithDetails, HttpContext.User);

            DonationDTOWithDetails donationDTOWithDetails = _mapper.Map<DonationDTOWithDetails>(donationWithDetails);
            return Ok(donationDTOWithDetails);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<DonationDTOWithDetails>>> GetFilteredDonations(string? shelterName, string? donatorName, decimal? minAmount, decimal? maxAmount, DateTime? dateAfter, DateTime? dateBefore, DonationStatus? status)
        {
            IEnumerable<Donation> donations = await _unitOfWork.DonationService.GetFilteredDonationsAsync(shelterName, donatorName, minAmount, maxAmount, dateAfter, dateBefore, status);
            IEnumerable<DonationDTOWithDetails> donationDTOs = _mapper.Map<IEnumerable<DonationDTOWithDetails>>(donations);
            return Ok(donationDTOs);
        }

        [Authorize(Policy = "MinimalAge")]
        [HttpPost]
        public async Task<ActionResult<DonationDTO>> CreateDonation(CreateDonationDTO donationDTO)
        {
            Donation donationToCreate = _mapper.Map<Donation>(donationDTO);
            Donation donationToCreateWithDonatorAndShelter = await _unitOfWork.DonationService.TryAddDonatorAndShelterToDonation(donationToCreate, donationDTO.UserId, donationDTO.ShelterId);

            Donation createdDonation = await _unitOfWork.DonationService.AddAsync(donationToCreateWithDonatorAndShelter);

            DonationDTO createdDonationDTO = _mapper.Map<DonationDTO>(createdDonation);
            return CreatedAtAction("GetDonation", new { id = createdDonation.Id }, createdDonationDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPut("{id}/updateDonationStatus")]
        public async Task<ActionResult<DonationDTO>> UpdateStatus(int id, [FromBody] DonationStatus newStatus)
        {
            Donation donationWithDetails = await _unitOfWork.DonationService.GetWithDetailsAsync(id);
            _permissionChecker.CheckPermissionForDonation(donationWithDetails, HttpContext.User);

            Donation updatedDonation = await _unitOfWork.DonationService.UpdateDonationStatus(id, newStatus);

            DonationDTO updatedDonationDTO = _mapper.Map<DonationDTO>(updatedDonation);
            return Ok(updatedDonationDTO);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonation(int id)
        {
            await _unitOfWork.DonationService.DeleteAsync(id);
            return NoContent();
        }
    }
}
