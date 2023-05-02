using Animal_Adoption_Management_System_Backend.Authorization;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionContractDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs;
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
    public class AdoptionContractController : ControllerBase
    {
        private readonly IAdoptionContractService _adoptionContractService;
        private readonly IManagedAdoptionContractService _managedAdoptionContractService;
        private readonly IAdoptionApplicationService _adoptionApplicationService;
        private readonly IAnimalService _animalService;
        private readonly IAnimalShelterService _animalShelterService;
        private readonly IMapper _mapper;
        private readonly IPermissionChecker _permissionChecker;


        public AdoptionContractController(
            IMapper mapper, 
            IPermissionChecker permissionChecker, 
            IAdoptionContractService adoptionContractService, 
            IManagedAdoptionContractService managedAdoptionContractService, 
            IAdoptionApplicationService adoptionApplicationService, 
            IAnimalService animalService, IAnimalShelterService animalShelterService)
        {
            _mapper = mapper;
            _permissionChecker = permissionChecker;
            _adoptionContractService = adoptionContractService;
            _managedAdoptionContractService = managedAdoptionContractService;
            _adoptionApplicationService = adoptionApplicationService;
            _animalService = animalService;
            _animalShelterService = animalShelterService;
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<AdoptionContractDTO>>> GetAllAdoptionContracts()
        {
            IEnumerable<AdoptionContract> adoptionContracts = await _adoptionContractService.GetAllAsync();
            IEnumerable<AdoptionContractDTO> adoptionContractDTOs = _mapper.Map<IEnumerable<AdoptionContractDTO>>(adoptionContracts);
            return Ok(adoptionContractDTOs);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet]
        public async Task<ActionResult<PagedResult<AdoptionContractDTO>>> GetPagedAdoptionContracts([FromQuery] QueryParameters queryParameters)
        {
            PagedResult<AdoptionContractDTO> pagedResult = await _adoptionContractService.GetAllAsync<AdoptionContractDTO>(queryParameters);
            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdoptionContractDTO>> GetAdoptionContract(int id)
        {
            AdoptionContract adoptionContractWithDetails = await _adoptionContractService.GetWithAnimalShelterDetailsAsync(id);
            _permissionChecker.CheckPermissionForAdoptionContract(adoptionContractWithDetails, HttpContext.User);

            AdoptionContractDTO adoptionContractDTO = _mapper.Map<AdoptionContractDTO>(adoptionContractWithDetails);
            return Ok(adoptionContractDTO);
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<AdoptionContractDTOWithManagerDetails>> GetAdoptionContractWithDetails(int id)
        {
            AdoptionContract adoptionContractWithDetails = await _adoptionContractService.GetWithAnimalShelterDetailsAsync(id);
            _permissionChecker.CheckPermissionForAdoptionContract(adoptionContractWithDetails, HttpContext.User);

            ManagedAdoptionContract managedAdoptionContract = await _managedAdoptionContractService.GetByAdoptionContractIdAsync(id);

            AdoptionContractDTOWithManagerDetails adoptionContractDTOWithManagerDetails = _mapper.Map<AdoptionContractDTOWithManagerDetails>(adoptionContractWithDetails);
            adoptionContractDTOWithManagerDetails.Manager = _mapper.Map<UserDTO>(managedAdoptionContract.Manager);

            return Ok(adoptionContractDTOWithManagerDetails);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<AdoptionContractDTOWithDetails>>> GetFilteredAdoptionContracts(string? animalName, string? applierName, DateTime? dateAfter, DateTime? dateBefore, bool? isActive)
        {
            IEnumerable<AdoptionContract> adoptionContracts = await _adoptionContractService.GetFilteredAdoptionContractsAsync(animalName, applierName, dateAfter, dateBefore, isActive);
            IEnumerable<AdoptionContractDTOWithDetails> adoptionContractDTOs = _mapper.Map<IEnumerable<AdoptionContractDTOWithDetails>>(adoptionContracts);
            return Ok(adoptionContractDTOs);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPost]
        public async Task<ActionResult<AdoptionContractDTO>> CreateAdoptionContract(CreateAdoptionContractDTO contractDTO)
        {
            AdoptionContract adoptionContractToCreate = _mapper.Map<AdoptionContract>(contractDTO);

            AdoptionContract adoptionContractToCreateWithRelations = await _adoptionContractService.TryAddRelatedEntitiesToAdoptionContract(adoptionContractToCreate, contractDTO.AnimalId, contractDTO.ApplierId);

            _permissionChecker.CheckPermissionForAnimal(adoptionContractToCreateWithRelations.Animal, User);

            // check for existing AdoptionApplication and set its Status to approved
            await _adoptionApplicationService.SetAdoptionApplicationStatusForContractCreation(adoptionContractToCreateWithRelations.Animal, adoptionContractToCreateWithRelations.Applier);

            AdoptionContract createdAdoptionContract = await _adoptionContractService.AddAsync(adoptionContractToCreateWithRelations);

            ManagedAdoptionContract managedAdoptionContractToCreate = await _managedAdoptionContractService.TryAddRelatedEntitiesToManagedContract(contractDTO.ManagerId, createdAdoptionContract);
            ManagedAdoptionContract createdManagedAdoptionContract = await _managedAdoptionContractService.AddAsync(managedAdoptionContractToCreate);

            //set AnimalStatus to adopted
            await _animalService.UpdateStatus(createdAdoptionContract.Animal.Id, AnimalStatus.Adopted);
            //close AnimalShelter connection with given exitdate (when the Animal is taken home from the Shelter)
            await _animalShelterService.CheckForAndCloseConnection(createdAdoptionContract.Animal, contractDTO.ExitDate);

            AdoptionContractDTOWithManagerDetails createdAdoptionContractDTO = _mapper.Map<AdoptionContractDTOWithManagerDetails>(createdAdoptionContract);
            createdAdoptionContractDTO.Manager = _mapper.Map<UserDTO>(createdManagedAdoptionContract.Manager);

            return CreatedAtAction("GetAdoptionContract", new { id = createdAdoptionContract.Id }, createdAdoptionContractDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPut("{id}/updateAdoptionContractIsActive")]
        public async Task<ActionResult<AdoptionContractDTO>> UpdateStatus(int id, [FromBody] bool isActive)
        {
            AdoptionContract adoptionContractWithDetails = await _adoptionContractService.GetWithAnimalShelterDetailsAsync(id);
            _permissionChecker.CheckPermissionForAdoptionContract(adoptionContractWithDetails, HttpContext.User);

            AdoptionContract updatedAdoptionContract = await _adoptionContractService.UpdateAdoptionContractIsActive(id, isActive);

            AdoptionContractDTO updatedAdoptionContractDTO = _mapper.Map<AdoptionContractDTO>(updatedAdoptionContract);
            return Ok(updatedAdoptionContractDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdoptionContract(int id)
        {
            await _adoptionContractService.DeleteAsync(id);
            return NoContent();
        }
    }
}
