using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalBreedDTOS;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AnimalBreedController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnimalBreedDTO>>> GetAllRBreeds()
        {
            IEnumerable<AnimalBreed> breeds = await _unitOfWork.AnimalBreedService.GetAllAsync();
            IEnumerable<AnimalBreedDTO> breedDTOs = _mapper.Map<IEnumerable<AnimalBreedDTO>>(breeds);
            return Ok(breedDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AnimalBreedDTO>> GetBreed(int id)
        {
            AnimalBreed breed = await _unitOfWork.AnimalBreedService.GetAsync(id);

            AnimalBreedDTO breedDTO = _mapper.Map<AnimalBreedDTO>(breed);
            return Ok(breedDTO);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<AnimalBreedDTO>>> GetFilteredBreeds(string? name, string? type, AnimalType? t)
        {
            //IEnumerable<AnimalBreed> breeds = await _unitOfWork.AnimalBreedService.GetFilteredBreedsAsync(name, type);
            IEnumerable<AnimalBreed> breeds = await _unitOfWork.AnimalBreedService.GetFilteredBreedsAsync(name, t);
            IEnumerable<AnimalBreedDTO> breedDTOs = _mapper.Map<IEnumerable<AnimalBreedDTO>>(breeds);
            return Ok(breedDTOs);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPost]
        public async Task<ActionResult<AnimalBreedDTO>> CreateBreed(CreateAnimalBreedDTO animalBreedDTO)
        {
            AnimalBreed breedToCreate = _mapper.Map<AnimalBreed>(animalBreedDTO);
            AnimalBreed createdBreed = await _unitOfWork.AnimalBreedService.AddAsync(breedToCreate);

            AnimalBreedDTO createdBreedDTO = _mapper.Map<AnimalBreedDTO>(createdBreed);
            return CreatedAtAction("GetBreed", new { id = createdBreed.Id }, createdBreedDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPut("{id}")]
        public async Task<ActionResult<AnimalBreedDTO>> UpdateBreed(int id, CreateAnimalBreedDTO animalBreedDTO)
        {
            AnimalBreed breedToUpdate = await _unitOfWork.AnimalBreedService.GetAsync(id);

            _mapper.Map(animalBreedDTO, breedToUpdate);

            try
            {
                await _unitOfWork.AnimalBreedService.UpdateAsync(breedToUpdate);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _unitOfWork.AnimalBreedService.Exists(id))
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
            await _unitOfWork.AnimalBreedService.DeleteAsync(id);
            return NoContent();
        }
    }
}
