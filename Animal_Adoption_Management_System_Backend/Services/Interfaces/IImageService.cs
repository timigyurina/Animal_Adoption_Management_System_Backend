using Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Repositories;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IImageService : IGenericRepository<Image>
    {
        Task<Image> AddWithPathAsync(Image imageToUpload, string imagePath);
        Task<string> SaveImageAsync(CreateImageDTO imageDTO);
    }
}
