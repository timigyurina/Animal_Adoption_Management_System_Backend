using Animal_Adoption_Management_System_Backend.Authorization;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalShelterDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ShelterDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Models.Pagination;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Animal_Adoption_Management_System_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalService _animalService;
        private readonly IAnimalBreedService _animalBreedService;
        private readonly IShelterService _shelterService;
        private readonly IAnimalShelterService _animalShelterService;
        private readonly IMapper _mapper;
        private readonly IPermissionChecker _permissionChecker;

        public AnimalController(IMapper mapper, IPermissionChecker permissionChecker, IAnimalService animalService, IAnimalBreedService animalBreedService, IShelterService shelterService, IAnimalShelterService animalShelterService)
        {
            _mapper = mapper;
            _permissionChecker = permissionChecker;
            _animalService = animalService;
            _animalBreedService = animalBreedService;
            _shelterService = shelterService;
            _animalShelterService = animalShelterService;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<AnimalDTO>>> GetAllAnimals()
        {
            IEnumerable<Animal> animals = await _animalService.GetAllAsync();
            IEnumerable<AnimalDTO> animalDTOs = _mapper.Map<IEnumerable<AnimalDTO>>(animals);
            return Ok(animalDTOs);
        }

        // GET: api/animal/?pagesize=25&pageNumber=1
        [HttpGet]
        public async Task<ActionResult<PagedResult<AnimalDTO>>> GetPagedAnimals([FromQuery] QueryParameters queryParameters)
        {
            PagedResult<AnimalDTO> pagedAnimalsResult = await _animalService.GetAllAsync<AnimalDTO>(queryParameters);
            return Ok(pagedAnimalsResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AnimalDTOWithInfoForAdopters>> GetAnimal(int id)
        {
            Animal animal = await _animalService.GetWithInfoForAdoptersAsync(id);
            AnimalShelter latestShelterConnection = _animalService.GetLatestShelterConnectionOfAnimal(animal);
            Shelter latestShelter = await _shelterService.GetWithAddressAsync(latestShelterConnection.Shelter.Id);

            ShelterDTOWithDetails latestShelterDTO = _mapper.Map<ShelterDTOWithDetails>(latestShelter);
            AnimalDTOWithInfoForAdopters animalDTO = _mapper.Map<AnimalDTOWithInfoForAdopters>(animal);

            animalDTO.LatestShelter = latestShelterDTO;
            animalDTO.EnrollmentDate = latestShelterConnection.EnrollmentDate;
            animalDTO.ExitDate = latestShelterConnection.ExitDate;

            return Ok(animalDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("{id}/details")]
        public async Task<ActionResult<AnimalDTOWithDetails>> GetAnimalWithDetails(int id)
        {
            Animal animalWithDetails = await _animalService.GetWithDetailsAsync(id);
            _permissionChecker.CheckPermissionForAnimal(animalWithDetails, HttpContext.User);

            AnimalDTOWithDetails animalDTOWithDetails = _mapper.Map<AnimalDTOWithDetails>(animalWithDetails);
            return Ok(animalDTOWithDetails);
        }

        [Authorize]
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<AnimalDTOWithBreed>>> GetFilteredAnimals(string? name, AnimalType? type, AnimalSize? size, AnimalStatus? status, Gender? gender, AnimalColor? color, int? breedId, bool? isSterilised, DateTime? bornAfter, DateTime? bornBefore)
        {
            IEnumerable<Animal> animals = await _animalService.GetFilteredAnimalsAsync(name, type, size, status, gender, color, breedId, isSterilised, bornAfter, bornBefore);
            IEnumerable<AnimalDTOWithBreed> animalDTOs = _mapper.Map<IEnumerable<AnimalDTOWithBreed>>(animals);
            return Ok(animalDTOs);
        }

        [Authorize]
        [HttpGet("pageAndFilter")]
        public async Task<ActionResult<IEnumerable<AnimalDTOWithBreed>>> GetPagedAndFilteredAnimals([FromQuery] QueryParameters queryParameters, string? name, AnimalType? type, AnimalSize? size, AnimalStatus? status, Gender? gender, AnimalColor? color, int? breedId, bool? isSterilised, DateTime? bornAfter, DateTime? bornBefore)
        {
            PagedResult<AnimalDTOWithBreed> animalDTOs = await _animalService.GetPagedAndFilteredAnimalsAsync<AnimalDTOWithBreed>(queryParameters, name, type, size, status, gender, color, breedId, isSterilised, bornAfter, bornBefore);
            return Ok(animalDTOs);
        }

        [HttpGet("{id}/image")]
        public async Task<ActionResult<AnimalDTOWithDetails>> GetAnimalImage(int id)
        {
            Animal animalWithImages = await _animalService.GetWithImagesAsync(id);
            AnimalDTOWithDetails animalDTOWithDetails = _mapper.Map<AnimalDTOWithDetails>(animalWithImages);
            return Ok(animalDTOWithDetails);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPost]
        public async Task<ActionResult<AnimalDTO>> CreateAnimal(CreateAnimalDTO animalDTO)
        {
            Animal animalToCreate = _mapper.Map<Animal>(animalDTO);
            AnimalBreed breedOfAnimal = await _animalBreedService.GetAsync(animalDTO.BreedId);

            animalToCreate.Breed = breedOfAnimal;
            Animal createdAnimal = await _animalService.AddAsync(animalToCreate);

            AnimalDTO createdAnimalDTO = _mapper.Map<AnimalDTO>(createdAnimal);
            return CreatedAtAction("GetAnimal", new { id = createdAnimal.Id }, createdAnimalDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")] 
        [HttpPost("{id}/addShelterConnection")]  // when adding a new Animal to the system or when Animal is taken back to (new) Shelter again
        public async Task<ActionResult<AnimalShelterDTO>> CreateAnimalShelterConnection(int id, CreateAnimalShelterDTO animaShelterlDTO)
        {
            // ShelterId is automatically added if User is ShelterEmployee, otherwise Admin can select it on the frontend
            Claim? shelterIdClaim = User.Claims.FirstOrDefault(c => c.Type == "ShelterId");

            //On frontend, animaShelterlDTO.ShelterId is -1, if Animal is created by employee, so it needs to be adjusted to Employee's Shelter. 
            int shelterId = shelterIdClaim != null ? int.Parse(shelterIdClaim.Value) : animaShelterlDTO.ShelterId; 
            if (shelterId == -1) throw new BadRequestException("No Shelter was provided");

            Shelter shelter = await _shelterService.GetWithAddressAsync(shelterId);

            // set Animal to be adoptable (if it was taken back to Shelter after being adopted)
            await _animalService.UpdateStatus(id, AnimalStatus.WaitingForAdoption);

            Animal animal = await _animalService.GetAsync(id);
            AnimalShelter createdConnection = await _animalShelterService.CreateAnimalShelterConnection(animal, shelter, animaShelterlDTO.EnrollmentDate);
            AnimalShelterDTO createdConnectionDTO = _mapper.Map<AnimalShelterDTO>(createdConnection);

            return Ok(createdConnectionDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPut("{id}/updateSterilisation")]
        public async Task<ActionResult<AnimalDTO>> UpdateSterilisation(int id, UpdateSterilisationDTO sterilisationDate)
        {
            Animal animalWithDetails = await _animalService.GetWithDetailsAsync(id);
            _permissionChecker.CheckPermissionForAnimal(animalWithDetails, HttpContext.User);

            Animal updatedAnimal = await _animalService.UpdateSterilisation(id, sterilisationDate);

            AnimalDTO updatedAnimalDTO = _mapper.Map<AnimalDTO>(updatedAnimal);
            return Ok(updatedAnimalDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPut("{id}/updateStatus")]
        public async Task<ActionResult<AnimalDTO>> UpdateStatus(int id, [FromBody] AnimalStatus newStatus)
        {
            Animal animalWithDetails = await _animalService.GetWithDetailsAsync(id);
            _permissionChecker.CheckPermissionForAnimal(animalWithDetails, HttpContext.User);

            Animal updatedAnimal = await _animalService.UpdateStatus(id, newStatus);

            AnimalDTO updatedAnimalDTO = _mapper.Map<AnimalDTO>(updatedAnimal);
            return Ok(updatedAnimalDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPut("{id}")]
        public async Task<ActionResult<AnimalDTOWithBreed>> UpdateAnimal(int id, UpdateAnimalDTO animalDTO)
        {
            Animal animalToUpdate = await _animalService.GetAsync(id);
            Animal animalWithDetails = await _animalService.GetWithDetailsAsync(id);
            _permissionChecker.CheckPermissionForAnimal(animalWithDetails, HttpContext.User);

            AnimalBreed breedOfAnimal = await _animalBreedService.GetAsync(animalDTO.BreedId);

            animalToUpdate.Breed = breedOfAnimal;
            _mapper.Map(animalDTO, animalToUpdate);

            await _animalService.UpdateAsync(animalToUpdate);

            AnimalDTOWithBreed updatedAnimalDTO = _mapper.Map<AnimalDTOWithBreed>(animalToUpdate);
            return Ok(updatedAnimalDTO);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            await _animalService.DeleteAsync(id);
            return NoContent();
        }
    }
}
