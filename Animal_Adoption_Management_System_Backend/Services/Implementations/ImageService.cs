using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Repositories;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class ImageService : GenericRepository<Image>, IImageService
    {
        private static readonly string WorkDir = AppDomain.CurrentDomain.BaseDirectory;
        public ImageService(AnimalAdoptionContext context) : base(context)
        {
        }
        public async Task<string> SaveImageAsync(CreateImageDTO imageDTO)
        {
            string filePath = CreateImageFilePath(imageDTO);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            await imageDTO.Image.CopyToAsync(new FileStream(filePath, FileMode.Create));

            return filePath;
        }
        public async Task<Image> AddWithPathAsync(Image imageToUpload, string imagePath)
        {
            imageToUpload.ImagePath = imagePath;
            return await AddAsync(imageToUpload);
        }

        public async Task<IEnumerable<Image>> GetFilteredImagesAsync(string? uploaderName, string? animalName, string? animalType, DateTime? takenBefore, DateTime? takenAfter)
        {
            IQueryable<Image> imageQuery = _context.Images
                .Include(i => i.Animal)
                .Include(i => i.Uploader)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(uploaderName))
            {
                imageQuery = imageQuery.Where(i => i.Uploader.FirstName.ToLower().Contains(uploaderName.ToLower()) || 
                                                   i.Uploader.LastName.ToLower().Contains(uploaderName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(animalName))
            {
                imageQuery = imageQuery.Where(i => i.Animal.Name.ToLower().Contains(animalName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(animalType))
            {
                bool animalTypeParsed = int.TryParse(animalType, out int animalTypeNumber);
                if (!animalTypeParsed || animalTypeNumber >= Enum.GetNames(typeof(AnimalType)).Length || animalTypeNumber < 0)
                    throw new BadRequestException($"Incorrect {nameof(AnimalType)}");

                imageQuery = imageQuery.Where(i => (int)i.Animal.Type == animalTypeNumber);
            }
            if (takenBefore != null)
            {
                imageQuery = imageQuery.Where(i => i.DateTaken < takenBefore);
            }
            if (takenAfter != null)
            {
                imageQuery = imageQuery.Where(i => i.DateTaken >= takenAfter);
            }

                return await imageQuery.ToListAsync();
        }

        public async Task<Image> GetWithDetailsAsync(int id)
        {
            if (!await Exists(id))
                throw new NotFoundException(typeof(Image).Name, id);

            return await _context.Images
                .Include(i => i.Animal)
                .Include(i => i.Uploader)
                .AsNoTracking()
                .FirstAsync(i => i.Id == id);
        }

        private static string CreateImageFilePath(CreateImageDTO imageDTO)
        {
            string uniqueFileName = GetUniqueFileName(imageDTO.Image.FileName);
            string uploads = Path.Combine(WorkDir, "Animals", "Images", imageDTO.AnimalId.ToString());
            string filePath = Path.Combine(uploads, uniqueFileName);
            return filePath;
        }

        private static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return string.Concat(Path.GetFileNameWithoutExtension(fileName)
                                , "_"
                                , Guid.NewGuid().ToString().AsSpan(0, 4)
                                , Path.GetExtension(fileName));
        }
    }
}
