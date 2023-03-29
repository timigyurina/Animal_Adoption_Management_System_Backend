using Animal_Adoption_Management_System_Backend.Models.DTOs.DonationDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Animal_Adoption_Management_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DonationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonationDTO>>> GetAllDonations()
        {
            IEnumerable<Donation> donations = await _unitOfWork.DonationService.GetAllAsync();
            IEnumerable<DonationDTO> donationDTOs = _mapper.Map<IEnumerable<DonationDTO>>(donations);
            return Ok(donationDTOs);
        }

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
            DonationDTOWithDetails donationDTOWithDetails = _mapper.Map<DonationDTOWithDetails>(donationWithDetails);
            return Ok(donationDTOWithDetails);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<DonationDTOWithDetails>>> GetFilteredDonations(string? shelterName, string? donatorName, decimal? minAmount, decimal? maxAmount, DateTime? dateAfter, DateTime? dateBefore, DonationStatus? status)
        {
            IEnumerable<Donation> donations = await _unitOfWork.DonationService.GetFilteredDonationsAsync(shelterName, donatorName, minAmount, maxAmount, dateAfter, dateBefore, status);
            IEnumerable<DonationDTOWithDetails> donationDTOs = _mapper.Map<IEnumerable<DonationDTOWithDetails>>(donations);
            return Ok(donationDTOs);
        }

        [HttpPost]
        public async Task<ActionResult<DonationDTO>> CreateDonation(CreateDonationDTO donationDTO)
        {
            Donation donationToCreate = _mapper.Map<Donation>(donationDTO);
            Donation donationToCreateWithDonatorAndShelter = await _unitOfWork.DonationService.TryAddDonatorAndShelterToDonation(donationToCreate, donationDTO.UserId, donationDTO.ShelterId);

            Donation createdDonation = await _unitOfWork.DonationService.AddAsync(donationToCreateWithDonatorAndShelter);

            DonationDTO createdDonationDTO = _mapper.Map<DonationDTO>(createdDonation);
            return CreatedAtAction("GetDonation", new { id = createdDonation.Id }, createdDonationDTO);
        }

        [HttpPut("{id}/updateDonationStatus")]
        public async Task<ActionResult<DonationDTO>> UpdateStatus(int id, [FromBody] DonationStatus newStatus)
        {
            Donation updatedDonation = await _unitOfWork.DonationService.UpdateDonationStatus(id, newStatus);

            DonationDTO updatedDonationDTO = _mapper.Map<DonationDTO>(updatedDonation);
            return Ok(updatedDonationDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonation(int id)
        {
            await _unitOfWork.DonationService.DeleteAsync(id);
            return NoContent();
        }
    }
}
