using Animal_Adoption_Management_System_Backend.Authorization;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Models.Pagination;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Image = Animal_Adoption_Management_System_Backend.Models.Entities.Image;

namespace Animal_Adoption_Management_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IAnimalService _animalService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IPermissionChecker _permissionChecker;

        public ImageController(IMapper mapper, IPermissionChecker permissionChecker, IImageService imageService, IAnimalService animalService, IUserService userService)
        {
            _mapper = mapper;
            _permissionChecker = permissionChecker;
            _imageService = imageService;
            _animalService = animalService;
            _userService = userService;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<ImageDTOWithAnimal>>> GetAllImages()
        {
            IEnumerable<Image> images = await _imageService.GetAllAsync(null, null, "Animal");
            IEnumerable<ImageDTOWithAnimal> imageDTOs = _mapper.Map<IEnumerable<ImageDTOWithAnimal>>(images);
            return Ok(imageDTOs);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ImageDTOWithAnimal>>> GetPagedImages([FromQuery] QueryParameters queryParameters)
        {
            PagedResult<ImageDTOWithAnimal> pagedResult = await _imageService.GetAllAsync<ImageDTOWithAnimal>(queryParameters, "Animal");
            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ImageDTOWithAnimal>> GetImage(int id)
        {
            Image imageWithAnimal = await _imageService.GetWithAnimalAsync(id);

            ImageDTOWithAnimal imageDTO = _mapper.Map<ImageDTOWithAnimal>(imageWithAnimal);
            return Ok(imageDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("{id}/details")]
        public async Task<ActionResult<ImageDTOWithDetails>> GetImageWithDetails(int id)
        {
            Image imageWithDetails = await _imageService.GetWithDetailsAsync(id);
            _permissionChecker.CheckPermissionForImage(imageWithDetails, User);

            ImageDTOWithDetails imageDTOWithDetails = _mapper.Map<ImageDTOWithDetails>(imageWithDetails);
            return Ok(imageDTOWithDetails);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ImageDTOWithAnimal>>> GetFilteredImages(string? uploaderName, string? animalName, AnimalType? animalType, DateTime? takenBefore, DateTime? takenAfter)
        {
            IEnumerable<Image> images = await _imageService.GetFilteredImagesAsync(uploaderName, animalName, animalType, takenBefore, takenAfter);
            IEnumerable<ImageDTOWithAnimal> imageDTOs = _mapper.Map<IEnumerable<ImageDTOWithAnimal>>(images);
            return Ok(imageDTOs);
        }

        [HttpGet("pageAndFilter")]
        public async Task<ActionResult<IEnumerable<ImageDTOWithAnimal>>> GetPagedAndFilteredImages([FromQuery] QueryParameters queryParameters, string? uploaderName, string? animalName, AnimalType? animalType, DateTime? takenBefore, DateTime? takenAfter)
        {
            PagedResult<ImageDTOWithAnimal> imageDTOs = await _imageService.GetPagedAndFilteredImagesAsync<ImageDTOWithAnimal>(queryParameters, uploaderName, animalName, animalType, takenBefore, takenAfter);
            return Ok(imageDTOs);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPost]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<ActionResult<ImageDTO>> CreateImage([FromForm] CreateImageDTO imageDTO)
        {
            Image imageToUpload = await MapAndAddEntities(imageDTO);
            _permissionChecker.CheckPermissionForAnimal(imageToUpload.Animal, User);

            string imagePath = await _imageService.SaveImageAsync(imageDTO);
            Image createdImage = await _imageService.AddWithPathAsync(imageToUpload, imagePath);

            ImageDTOWithAnimal createdImageDTO = _mapper.Map<ImageDTOWithAnimal>(createdImage);
            return Ok(createdImageDTO);
        }

        private async Task<Image> MapAndAddEntities(CreateImageDTO imageDTO)
        {
            if (string.IsNullOrEmpty(Request.GetMultipartBoundary()))
                throw new BadRequestException("Invalid post header");

            Animal animal = await _animalService.GetWithAnimalShelterDetailsAsync(imageDTO.AnimalId);
            User uploader = await _userService.GetAsync(User.Claims.First(c => c.Type == "UserId").Value);

            Image imageToUpload = _mapper.Map<Image>(imageDTO);
            imageToUpload.Animal = animal;
            imageToUpload.Uploader = uploader;
            if (imageDTO.DateTaken == null)
                imageToUpload.DateTaken = DateTime.Now;

            return imageToUpload;
        }
    }
}
