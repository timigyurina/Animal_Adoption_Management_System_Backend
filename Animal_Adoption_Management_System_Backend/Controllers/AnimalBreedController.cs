using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalBreedDTOS;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Models.Pagination;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Animal_Adoption_Management_System_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalBreedController : ControllerBase
    {
        private readonly IAnimalBreedService _animalBreedService;
        private readonly IMapper _mapper;

        public AnimalBreedController(IMapper mapper, IAnimalBreedService animalBreedService)
        {
            _mapper = mapper;
            _animalBreedService = animalBreedService;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<AnimalBreedDTO>>> GetAllRBreeds()
        {
            IEnumerable<AnimalBreed> breeds = await _animalBreedService.GetAllAsync();
            IEnumerable<AnimalBreedDTO> breedDTOs = _mapper.Map<IEnumerable<AnimalBreedDTO>>(breeds);
            return Ok(breedDTOs);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<AnimalBreedDTO>>> GetPagedAnimalBreeds([FromQuery] QueryParameters queryParameters)
        {
            PagedResult<AnimalBreedDTO> pagedAnimalBreedsResult = await _animalBreedService.GetAllAsync<AnimalBreedDTO>(queryParameters);
            return Ok(pagedAnimalBreedsResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AnimalBreedDTO>> GetBreed(int id)
        {
            AnimalBreed breed = await _animalBreedService.GetAsync(id);

            AnimalBreedDTO breedDTO = _mapper.Map<AnimalBreedDTO>(breed);
            return Ok(breedDTO);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<AnimalBreedDTO>>> GetFilteredBreeds(string? name, AnimalType? type)
        {
            IEnumerable<AnimalBreed> breeds = await _animalBreedService.GetFilteredBreedsAsync(name, type);
            IEnumerable<AnimalBreedDTO> breedDTOs = _mapper.Map<IEnumerable<AnimalBreedDTO>>(breeds);
            return Ok(breedDTOs);
        }

        [HttpGet("pageAndFilter")]
        public async Task<ActionResult<IEnumerable<AnimalBreedDTO>>> GetPagedAndFilteredBreeds([FromQuery] QueryParameters queryParameters, string? name, AnimalType? type)
        {
            PagedResult<AnimalBreedDTO> breedDTOs = await _animalBreedService.GetPagedAndFilteredBreedsAsync<AnimalBreedDTO>(queryParameters, name, type);
            return Ok(breedDTOs);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPost]
        public async Task<ActionResult<AnimalBreedDTO>> CreateBreed(CreateAnimalBreedDTO animalBreedDTO)
        {
            AnimalBreed breedToCreate = _mapper.Map<AnimalBreed>(animalBreedDTO);
            AnimalBreed createdBreed = await _animalBreedService.AddAsync(breedToCreate);

            AnimalBreedDTO createdBreedDTO = _mapper.Map<AnimalBreedDTO>(createdBreed);
            return CreatedAtAction("GetBreed", new { id = createdBreed.Id }, createdBreedDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPut("{id}")]
        public async Task<ActionResult<AnimalBreedDTO>> UpdateBreed(int id, CreateAnimalBreedDTO animalBreedDTO)
        {
            AnimalBreed breedToUpdate = await _animalBreedService.GetAsync(id);

            _mapper.Map(animalBreedDTO, breedToUpdate);

            try
            {
                await _animalBreedService.UpdateAsync(breedToUpdate);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _animalBreedService.Exists(id))
                    throw new NotFoundException(typeof(AnimalBreed).Name, id);
                else
                    throw;
            }

            AnimalBreedDTO updatedBreed = _mapper.Map<AnimalBreedDTO>(breedToUpdate);
            return Ok(updatedBreed);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBreed(int id)
        {
            await _animalBreedService.DeleteAsync(id);
            return NoContent();
        }
    }
}
