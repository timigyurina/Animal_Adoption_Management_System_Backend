using Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionContractDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Animal_Adoption_Management_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdoptionContractController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdoptionContractController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdoptionContractDTO>>> GetAllAdoptionContracts()
        {
            IEnumerable<AdoptionContract> adoptionContracts = await _unitOfWork.AdoptionContractService.GetAllAsync();
            IEnumerable<AdoptionContractDTO> adoptionContractDTOs = _mapper.Map<IEnumerable<AdoptionContractDTO>>(adoptionContracts);
            return Ok(adoptionContractDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdoptionContractDTO>> GetAdoptionContract(int id)
        {
            AdoptionContract adoptionContract = await _unitOfWork.AdoptionContractService.GetAsync(id);

            AdoptionContractDTO adoptionContractDTO = _mapper.Map<AdoptionContractDTO>(adoptionContract);
            return Ok(adoptionContractDTO);
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<AdoptionContractDTOWithManagerDetails>> GetAdoptionContractWithDetails(int id)
        {
            AdoptionContract adoptionContractWithDetails = await _unitOfWork.AdoptionContractService.GetWithDetailsAsync(id);
            ManagedAdoptionContract managedAdoptionContract = await _unitOfWork.ManagedAdoptionContractService.GetByAdoptionContractIdAsync(id);

            AdoptionContractDTOWithManagerDetails adoptionContractDTOWithManagerDetails = _mapper.Map<AdoptionContractDTOWithManagerDetails>(adoptionContractWithDetails);
            adoptionContractDTOWithManagerDetails.Manager = _mapper.Map<UserDTO>(managedAdoptionContract.Manager);

            return Ok(adoptionContractDTOWithManagerDetails);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<AdoptionContractDTOWithDetails>>> GetFilteredAdoptionContracts(string? animalName, string? applierName, DateTime? dateAfter, DateTime? dateBefore, bool? isActive)
        {
            IEnumerable<AdoptionContract> adoptionContracts = await _unitOfWork.AdoptionContractService.GetFilteredAdoptionContractsAsync(animalName, applierName, dateAfter, dateBefore, isActive);
            IEnumerable<AdoptionContractDTOWithDetails> adoptionContractDTOs = _mapper.Map<IEnumerable<AdoptionContractDTOWithDetails>>(adoptionContracts);
            return Ok(adoptionContractDTOs);
        }

        [HttpPost]
        public async Task<ActionResult<AdoptionContractDTO>> CreateAdoptionContract(CreateAdoptionContractDTO contractDTO)
        {
            AdoptionContract adoptionContractToCreate = _mapper.Map<AdoptionContract>(contractDTO);

            AdoptionContract adoptionContractToCreateWithRelations = await _unitOfWork.AdoptionContractService.TryAddRelatedEntitiesToAdoptionContract(adoptionContractToCreate, contractDTO.AnimalId, contractDTO.ApplierId);
            // check for existing AdoptionApplication and set its Status to approved
            await _unitOfWork.AdoptionApplicationService.SetAdoptionApplicationStatusForContractCreation(adoptionContractToCreateWithRelations.Animal, adoptionContractToCreateWithRelations.Applier);

            AdoptionContract createdAdoptionContract = await _unitOfWork.AdoptionContractService.AddAsync(adoptionContractToCreateWithRelations);

            ManagedAdoptionContract managedAdoptionContractToCreate = await _unitOfWork.ManagedAdoptionContractService.TryAddRelatedEntitiesToManagedContract(contractDTO.ManagerId, createdAdoptionContract);
            ManagedAdoptionContract createdManagedAdoptionContract = await _unitOfWork.ManagedAdoptionContractService.AddAsync(managedAdoptionContractToCreate);

            //set AnimalStatus to adopted
            await _unitOfWork.AnimalService.UpdateStatus(createdAdoptionContract.Animal.Id, AnimalStatus.Adopted);
            //close AnimalShelter connection with given exitdate (when the Animal is taken home from the Shelter)
            await _unitOfWork.AnimalShelterService.CheckForAndCloseConnection(createdAdoptionContract.Animal, contractDTO.ExitDate);

            AdoptionContractDTOWithManagerDetails createdAdoptionContractDTO = _mapper.Map<AdoptionContractDTOWithManagerDetails>(createdAdoptionContract);
            createdAdoptionContractDTO.Manager = _mapper.Map<UserDTO>(createdManagedAdoptionContract.Manager);

            return CreatedAtAction("GetAdoptionContract", new { id = createdAdoptionContract.Id }, createdAdoptionContractDTO);
        }

        [HttpPut("{id}/updateAdoptionContractIsActive")]
        public async Task<ActionResult<AdoptionContractDTO>> UpdateStatus(int id, [FromBody] bool isActive)
        {
            AdoptionContract updatedAdoptionContract = await _unitOfWork.AdoptionContractService.UpdateAdoptionContractIsActive(id, isActive);

            AdoptionContractDTO updatedAdoptionContractDTO = _mapper.Map<AdoptionContractDTO>(updatedAdoptionContract);
            return Ok(updatedAdoptionContractDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdoptionContract(int id)
        {
            await _unitOfWork.AdoptionContractService.DeleteAsync(id);
            return NoContent();
        }
    }
}
