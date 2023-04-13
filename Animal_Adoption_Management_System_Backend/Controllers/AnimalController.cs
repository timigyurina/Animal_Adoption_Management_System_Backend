using Animal_Adoption_Management_System_Backend.Authorization;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalShelterDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Animal_Adoption_Management_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPermissionChecker _permissionChecker;

        public AnimalController(IUnitOfWork unitOfWork, IMapper mapper, IPermissionChecker permissionChecker)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _permissionChecker = permissionChecker;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnimalDTO>>> GetAllAnimals()
        {
            IEnumerable<Animal> animals = await _unitOfWork.AnimalService.GetAllAsync();
            IEnumerable<AnimalDTO> animalDTOs = _mapper.Map<IEnumerable<AnimalDTO>>(animals);
            return Ok(animalDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AnimalDTO>> GetAnimal(int id)
        {
            Animal animal = await _unitOfWork.AnimalService.GetAsync(id);

            AnimalDTO animalDTO = _mapper.Map<AnimalDTO>(animal);
            return Ok(animalDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("{id}/details")]
        public async Task<ActionResult<AnimalDTOWithDetails>> GetAnimalWithDetails(int id)
        {
            Animal animalWithDetails = await _unitOfWork.AnimalService.GetWithDetailsAsync(id);
            _permissionChecker.CheckPermissionForAnimal(animalWithDetails, HttpContext.User);

            AnimalDTOWithDetails animalDTOWithDetails = _mapper.Map<AnimalDTOWithDetails>(animalWithDetails);
            return Ok(animalDTOWithDetails);
        }

        [Authorize]
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<AnimalDTO>>> GetFilteredAnimals(string? name, string? type, string? size, string? status, string? gender, string? color, int? breedId, bool? isSterilised, DateTime? bornAfter, DateTime? bornBefore)
        {
            IEnumerable<Animal> animals = await _unitOfWork.AnimalService.GetFilteredAnimalsAsync(name, type, size, status, gender, color, breedId, isSterilised, bornAfter, bornBefore);
            IEnumerable<AnimalDTO> animalDTOs = _mapper.Map<IEnumerable<AnimalDTO>>(animals);
            return Ok(animalDTOs);
        }

        [HttpGet("{id}/image")]
        public async Task<ActionResult<AnimalDTOWithDetails>> GetAnimalImage(int id)
        {
            Animal animalWithImages = await _unitOfWork.AnimalService.GetWithImagesAsync(id);
            AnimalDTOWithDetails animalDTOWithDetails = _mapper.Map<AnimalDTOWithDetails>(animalWithImages);
            return Ok(animalDTOWithDetails);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPost]
        public async Task<ActionResult<AnimalDTO>> CreateAnimal(CreateAnimalDTO animalDTO)
        {
            Animal animalToCreate = _mapper.Map<Animal>(animalDTO);
            AnimalBreed breedOfAnimal = await _unitOfWork.AnimalBreedService.GetAsync(animalDTO.BreedId);

            animalToCreate.Breed = breedOfAnimal;
            Animal createdAnimal = await _unitOfWork.AnimalService.AddAsync(animalToCreate);

            AnimalDTO createdAnimalDTO = _mapper.Map<AnimalDTO>(createdAnimal);
            return CreatedAtAction("GetAnimal", new { id = createdAnimal.Id }, createdAnimalDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")] // frontend automatically adds logged in User's ShelterId when adding new Animal
        [HttpPost("{id}/addShelterConnection")]  // when adding a new Animal to the system or when Animal is taken back to (new) Shelter again
        public async Task<ActionResult<AnimalShelterDTO>> CreateAnimalShelterConnection(int id, CreateAnimalShelterDTO animaShelterlDTO)
        {
            Animal animal = await _unitOfWork.AnimalService.GetAsync(id);
            Shelter shelter = await _unitOfWork.ShelterService.GetAsync(animaShelterlDTO.ShelterId);
            // set Animal to be adoptable (if it was taken back to Shelter after being adopted)
            await _unitOfWork.AnimalService.UpdateStatus(id, AnimalStatus.WaitingForAdoption);

            AnimalShelter createdConnection = await _unitOfWork.AnimalShelterService.CreateAnimalShelterConnection(animal, shelter, animaShelterlDTO.EnrollmentDate);
            AnimalShelterDTO createdConnectionDTO = _mapper.Map<AnimalShelterDTO>(createdConnection);

            return Ok(createdConnectionDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPut("{id}/updateSterilisation")]
        public async Task<ActionResult<AnimalDTO>> UpdateSterilisation(int id, UpdateSterilisationDTO sterilisationDate)
        {
            Animal animalWithDetails = await _unitOfWork.AnimalService.GetWithDetailsAsync(id);
            _permissionChecker.CheckPermissionForAnimal(animalWithDetails, HttpContext.User);

            Animal updatedAnimal = await _unitOfWork.AnimalService.UpdateSterilisation(id, sterilisationDate);

            AnimalDTO updatedAnimalDTO = _mapper.Map<AnimalDTO>(updatedAnimal);
            return Ok(updatedAnimalDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPut("{id}/updateStatus")]
        public async Task<ActionResult<AnimalDTO>> UpdateStatus(int id, [FromBody] AnimalStatus newStatus)
        {
            Animal animalWithDetails = await _unitOfWork.AnimalService.GetWithDetailsAsync(id);
            _permissionChecker.CheckPermissionForAnimal(animalWithDetails, HttpContext.User);

            Animal updatedAnimal = await _unitOfWork.AnimalService.UpdateStatus(id, newStatus);

            AnimalDTO updatedAnimalDTO = _mapper.Map<AnimalDTO>(updatedAnimal);
            return Ok(updatedAnimalDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPut("{id}")]
        public async Task<ActionResult<AnimalDTO>> UpdateAnimal(int id, UpdateAnimalDTO animalDTO)
        {
            Animal animalToUpdate = await _unitOfWork.AnimalService.GetAsync(id);
            Animal animalWithDetails = await _unitOfWork.AnimalService.GetWithDetailsAsync(id);
            _permissionChecker.CheckPermissionForAnimal(animalWithDetails, HttpContext.User);

            AnimalBreed breedOfAnimal = await _unitOfWork.AnimalBreedService.GetAsync(animalDTO.BreedId);

            animalToUpdate.Breed = breedOfAnimal;
            _mapper.Map(animalDTO, animalToUpdate);

            await _unitOfWork.AnimalService.UpdateAsync(animalToUpdate);

            AnimalDTO updatedAnimalDTO = _mapper.Map<AnimalDTO>(animalToUpdate);
            return Ok(updatedAnimalDTO);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            await _unitOfWork.AnimalService.DeleteAsync(id);
            return NoContent();
        }
    }
}
