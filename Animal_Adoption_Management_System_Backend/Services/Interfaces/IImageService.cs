using Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Models.Pagination;
using Animal_Adoption_Management_System_Backend.Repositories;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IImageService : IGenericRepository<Image>
    {
        Task<Image> AddWithPathAsync(Image imageToUpload, string imagePath);
        Task<IEnumerable<Image>> GetFilteredImagesAsync(string? uploaderName, string? animalName, AnimalType? animalType, DateTime? takenBefore, DateTime? takenAfter);
        Task<Image> GetWithAnimalAsync(int id);
        Task<Image> GetWithDetailsAsync(int id);
        Task<string> SaveImageAsync(CreateImageDTO imageDTO);
        Task<PagedResult<TResult>> GetPagedAndFilteredImagesAsync<TResult>(QueryParameters queryParameters, string? uploaderName, string? animalName, AnimalType? animalType, DateTime? takenBefore, DateTime? takenAfter);
    }
}
