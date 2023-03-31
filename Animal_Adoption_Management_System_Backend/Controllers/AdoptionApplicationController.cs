﻿using Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionApplicationDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Animal_Adoption_Management_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdoptionApplicationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdoptionApplicationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdoptionApplicationDTO>>> GetAllAdoptionApplications()
        {
            IEnumerable<AdoptionApplication> adoptionApplications = await _unitOfWork.AdoptionApplicationService.GetAllAsync();
            IEnumerable<AdoptionApplicationDTO> adoptionApplicationDTOs = _mapper.Map<IEnumerable<AdoptionApplicationDTO>>(adoptionApplications);
            return Ok(adoptionApplicationDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdoptionApplicationDTO>> GetAdoptionApplication(int id)
        {
            AdoptionApplication adoptionApplication = await _unitOfWork.AdoptionApplicationService.GetAsync(id);

            AdoptionApplicationDTO adoptionApplicationDTO = _mapper.Map<AdoptionApplicationDTO>(adoptionApplication);
            return Ok(adoptionApplicationDTO);
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<AdoptionApplicationDTOWithDetails>> GetAdoptionApplicationWithDetails(int id)
        {
            AdoptionApplication adoptionApplicationWithDetails = await _unitOfWork.AdoptionApplicationService.GetWithDetailsAsync(id);
            AdoptionApplicationDTOWithDetails adoptionApplicationDTOWithDetails = _mapper.Map<AdoptionApplicationDTOWithDetails>(adoptionApplicationWithDetails);
            return Ok(adoptionApplicationDTOWithDetails);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<AdoptionApplicationDTOWithDetails>>> GetFilteredAdoptionApplications(string? animalName, string? applierName, DateTime? dateAfter, DateTime? dateBefore, ApplicationStatus? status)
        {
            IEnumerable<AdoptionApplication> adoptionApplications = await _unitOfWork.AdoptionApplicationService.GetFilteredAdoptionApplicationsAsync(animalName, applierName, dateAfter, dateBefore, status);
            IEnumerable<AdoptionApplicationDTOWithDetails> adoptionApplicationDTOs = _mapper.Map<IEnumerable<AdoptionApplicationDTOWithDetails>>(adoptionApplications);
            return Ok(adoptionApplicationDTOs);
        }

        [HttpPost]
        public async Task<ActionResult<AdoptionApplicationDTO>> CreateAdoptionApplication(CreateAdoptionApplicationDTO applicationDTO)
        {
            AdoptionApplication adoptionApplicationToCreate = _mapper.Map<AdoptionApplication>(applicationDTO);
            AdoptionApplication adoptionApplicationToCreateWithAnimalAndApplier = await _unitOfWork.AdoptionApplicationService.TryAddAnimalAndApplierToAdoptionApplication(adoptionApplicationToCreate, applicationDTO.AnimalId, applicationDTO.ApplierId);

            AdoptionApplication createdAdoptionApplication = await _unitOfWork.AdoptionApplicationService.AddAsync(adoptionApplicationToCreateWithAnimalAndApplier);

            AdoptionApplicationDTO createdAdoptionApplicationDTO = _mapper.Map<AdoptionApplicationDTO>(createdAdoptionApplication);
            return CreatedAtAction("GetAdoptionApplication", new { id = createdAdoptionApplication.Id }, createdAdoptionApplicationDTO);
        }

        [HttpPut("{id}/updateAdoptionApplicationStatus")]
        public async Task<ActionResult<AdoptionApplicationDTO>> UpdateStatus(int id, [FromBody] ApplicationStatus newStatus)
        {
            AdoptionApplication updatedAdoptionApplication = await _unitOfWork.AdoptionApplicationService.UpdateAdoptionApplicationStatus(id, newStatus);

            AdoptionApplicationDTO updatedAdoptionApplicationDTO = _mapper.Map<AdoptionApplicationDTO>(updatedAdoptionApplication);
            return Ok(updatedAdoptionApplicationDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdoptionApplication(int id)
        {
            await _unitOfWork.AdoptionApplicationService.DeleteAsync(id);
            return NoContent();
        }

    }
}