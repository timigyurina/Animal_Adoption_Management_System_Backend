using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalBreed;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Animal_Adoption_Management_System_Backend.Controllers
{
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
    }
}
