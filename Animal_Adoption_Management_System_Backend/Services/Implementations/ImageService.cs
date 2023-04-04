using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Repositories;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;

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


        private string CreateImageFilePath(CreateImageDTO imageDTO)
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
