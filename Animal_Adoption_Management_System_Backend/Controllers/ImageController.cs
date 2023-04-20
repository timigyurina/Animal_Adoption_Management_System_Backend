using Animal_Adoption_Management_System_Backend.Authorization;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPermissionChecker _permissionChecker;

        public ImageController(IUnitOfWork unitOfWork, IMapper mapper, IPermissionChecker permissionChecker)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _permissionChecker = permissionChecker;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<ImageDTOWithDetails>>> GetAllImages()
        {
            IEnumerable<Image> images = await _unitOfWork.ImageService.GetAllAsync(null, null, "Animal");
            IEnumerable<ImageDTOWithDetails> imageDTOs = _mapper.Map<IEnumerable<ImageDTOWithDetails>>(images);
            return Ok(imageDTOs);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ImageDTOWithDetails>>> GetPagedImages([FromQuery] QueryParameters queryParameters)
        {
            PagedResult<ImageDTOWithDetails> pagedResult = await _unitOfWork.ImageService.GetAllAsync<ImageDTOWithDetails>(queryParameters, "Animal");
            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ImageDTOWithDetails>> GetImage(int id)
        {
            Image imageWithAnimal = await _unitOfWork.ImageService.GetWithAnimalAsync(id);

            ImageDTOWithDetails imageDTO = _mapper.Map<ImageDTOWithDetails>(imageWithAnimal);
            return Ok(imageDTO);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("{id}/details")]
        public async Task<ActionResult<ImageDTOWithDetails>> GetImageWithDetails(int id)
        {
            Image imageWithDetails = await _unitOfWork.ImageService.GetWithDetailsAsync(id);
            _permissionChecker.CheckPermissionForImage(imageWithDetails, User);

            ImageDTOWithDetails imageDTOWithDetails = _mapper.Map<ImageDTOWithDetails>(imageWithDetails);
            return Ok(imageDTOWithDetails);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ImageDTOWithDetails>>> GetFilteredImages(string? uploaderName, string? animalName, string? animalType, DateTime? takenBefore, DateTime? takenAfter)
        {
            IEnumerable<Image> images = await _unitOfWork.ImageService.GetFilteredImagesAsync(uploaderName, animalName, animalType, takenBefore, takenAfter);
            IEnumerable<ImageDTOWithDetails> imageDTOs = _mapper.Map<IEnumerable<ImageDTOWithDetails>>(images);
            return Ok(imageDTOs);
        }

        [Authorize(Roles = "Administrator, ShelterEmployee")]
        [HttpPost]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<ActionResult<ImageDTO>> CreateImage([FromForm] CreateImageDTO imageDTO)
        {
            Image imageToUpload = await MapAndAddEntities(imageDTO);
            _permissionChecker.CheckPermissionForAnimal(imageToUpload.Animal, User);

            string imagePath = await _unitOfWork.ImageService.SaveImageAsync(imageDTO);
            Image createdImage = await _unitOfWork.ImageService.AddWithPathAsync(imageToUpload, imagePath);

            ImageDTO createdImageDTO = _mapper.Map<ImageDTO>(createdImage);
            return Ok(createdImageDTO);
        }

        private async Task<Image> MapAndAddEntities(CreateImageDTO imageDTO)
        {
            if (string.IsNullOrEmpty(Request.GetMultipartBoundary()))
                throw new BadRequestException("Invalid post header");

            Animal animal = await _unitOfWork.AnimalService.GetWithAnimalShelterDetailsAsync(imageDTO.AnimalId);
            User uploader = await _unitOfWork.UserService.GetAsync(imageDTO.UploaderId);

            Image imageToUpload = _mapper.Map<Image>(imageDTO);
            imageToUpload.Animal = animal;
            imageToUpload.Uploader = uploader;
            if (imageDTO.DateTaken == null)
                imageToUpload.DateTaken = DateTime.Now;

            return imageToUpload;
        }
    }
}
