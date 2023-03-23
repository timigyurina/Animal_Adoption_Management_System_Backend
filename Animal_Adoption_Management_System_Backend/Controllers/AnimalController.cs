using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalBreedDTOS;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalShelterDTOs;
using Microsoft.EntityFrameworkCore;
using Animal_Adoption_Management_System_Backend.Models.Enums;

namespace Animal_Adoption_Management_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AnimalController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            Animal? animal = await _unitOfWork.AnimalService.GetAsync(id);
            if (animal == null)
                throw new NotFoundException(typeof(Animal).Name, id);

            AnimalDTO animalDTO = _mapper.Map<AnimalDTO>(animal);
            return Ok(animalDTO);
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<AnimalDTOWithDetails>> GetAnimalWithDetails(int id)
        {
            Animal animalWithDetails = await _unitOfWork.AnimalService.GetWithDetailsAsync(id);
            AnimalDTOWithDetails animalDTOWithDetails = _mapper.Map<AnimalDTOWithDetails>(animalWithDetails);
            return Ok(animalDTOWithDetails);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<AnimalDTO>>> GetFilteredAnimals(string? name, string? type, string? size, string? status, string? gender, string? color, int? breedId, bool? isSterilised, DateTime? bornAfter, DateTime? bornBefore)
        {
            IEnumerable<Animal> animals = await _unitOfWork.AnimalService.GetFilteredAnimalsAsync(name, type, size, status, gender, color, breedId, isSterilised, bornAfter, bornBefore);
            IEnumerable<AnimalDTO> animalDTOs = _mapper.Map<IEnumerable<AnimalDTO>>(animals);
            return Ok(animalDTOs);
        }

        [HttpPost]
        public async Task<ActionResult<AnimalDTO>> CreateAnimal(CreateAnimalDTO animalDTO)
        {
            Animal animalToCreate = _mapper.Map<Animal>(animalDTO);
            AnimalBreed? breedOfAnimal = await _unitOfWork.AnimalBreedService.GetAsync(animalDTO.BreedId);
            if (breedOfAnimal == null)
                throw new NotFoundException(typeof(AnimalBreed).Name, animalDTO.BreedId);
            animalToCreate.Breed = breedOfAnimal;

            Animal createdAnimal = await _unitOfWork.AnimalService.AddAsync(animalToCreate);

            AnimalDTO createdAnimalDTO = _mapper.Map<AnimalDTO>(createdAnimal);
            return CreatedAtAction("GetAnimal", new { id = createdAnimal.Id }, createdAnimalDTO);
        }

        [HttpPost("{id}/addShelter")]
        public async Task<ActionResult<AnimalShelterDTO>> CreateAnimalShelterConnection(int id, CreateAnimalShelterDTO animaShelterlDTO)
        {
            Animal? animal = await _unitOfWork.AnimalService.GetAsync(id);
            if (animal == null)
                throw new NotFoundException(typeof(Animal).Name, id);
            Shelter? shelter = await _unitOfWork.ShelterService.GetAsync(animaShelterlDTO.ShelterId);
            if (shelter == null)
                throw new NotFoundException(typeof(Shelter).Name, animaShelterlDTO.ShelterId);

            AnimalShelter createdConnection = await _unitOfWork.AnimalShelterService.CreateAnimalShelterConnection(animal, shelter, animaShelterlDTO.EnrollmentDate);
            AnimalShelterDTO createdConnectionDTO = _mapper.Map<AnimalShelterDTO>(createdConnection);

            return Ok(createdConnectionDTO);
        }

        [HttpPut("{id}/updateSterilisation")]
        public async Task<ActionResult<AnimalDTO>> UpdateSterilisation(int id, UpdateSterilisationDTO sterilisationDate)
        {
            Animal updatedAnimal = await _unitOfWork.AnimalService.UpdateSterilisation(id, sterilisationDate);

            AnimalDTO updatedAnimalDTO = _mapper.Map<AnimalDTO>(updatedAnimal);
            return Ok(updatedAnimalDTO);
        }
        
        [HttpPut("{id}/updateStatus")]
        public async Task<ActionResult<AnimalDTO>> UpdateStatus(int id, [FromBody]AnimalStatus newStatus)
        {
            Animal updatedAnimal = await _unitOfWork.AnimalService.UpdateStatus(id, newStatus);

            AnimalDTO updatedAnimalDTO = _mapper.Map<AnimalDTO>(updatedAnimal);
            return Ok(updatedAnimalDTO);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<AnimalDTO>> UpdateAnimal(int id, UpdateAnimalDTO animalDTO)
        {
            Animal? animal = await _unitOfWork.AnimalService.GetAsync(id);
            if (animal == null)
                throw new NotFoundException(typeof(Animal).Name, id);
            AnimalBreed? breedOfAnimal = await _unitOfWork.AnimalBreedService.GetAsync(animalDTO.BreedId);
            if (breedOfAnimal == null)
                throw new NotFoundException(typeof(AnimalBreed).Name, animalDTO.BreedId);
            animal.Breed = breedOfAnimal;

            _mapper.Map(animalDTO, animal);

            await _unitOfWork.AnimalService.UpdateAsync(animal);

            AnimalDTO updatedAnimalDTO = _mapper.Map<AnimalDTO>(animal);
            return Ok(updatedAnimalDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            Animal? animalToDelete = await _unitOfWork.AnimalService.GetAsync(id);
            if (animalToDelete == null)
                throw new NotFoundException(typeof(Animal).Name, id);

            await _unitOfWork.AnimalService.DeleteAsync(id);

            return NoContent();
        }
    }
}
