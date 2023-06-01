using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Models.Pagination;
using Animal_Adoption_Management_System_Backend.Repositories;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class ImageService : GenericRepository<Image>, IImageService
    {
        private readonly IHostEnvironment _env;

        public ImageService(AnimalAdoptionContext context, IMapper mapper, IHostEnvironment env) : base(context, mapper)
        {
            _env = env;
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

        public async Task<IEnumerable<Image>> GetFilteredImagesAsync(string? uploaderName, string? animalName, AnimalType? animalType, DateTime? takenBefore, DateTime? takenAfter)
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
            if (animalType != null)
            {
                imageQuery = imageQuery.Where(i => i.Animal.Type == animalType);
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
                    .ThenInclude(a => a.AnimalShelters)
                        .ThenInclude(s => s.Shelter)
                .Include(i => i.Uploader)
                .AsNoTracking()
                .FirstAsync(i => i.Id == id);
        }

        public async Task<Image> GetWithAnimalAsync(int id)
        {
            if (!await Exists(id))
                throw new NotFoundException(typeof(Image).Name, id);

            return await _context.Images
                .Include(i => i.Animal)
                .AsNoTracking()
                .FirstAsync(i => i.Id == id);
        }

        public async Task<PagedResult<TResult>> GetPagedAndFilteredImagesAsync<TResult>(QueryParameters queryParameters, string? uploaderName, string? animalName, AnimalType? animalType, DateTime? takenBefore, DateTime? takenAfter)
        {
            List<Expression<Func<Image, bool>>> filters = new();

            if (!string.IsNullOrWhiteSpace(uploaderName))
            {
                Expression<Func<Image, bool>> uploaderNamePredicate = i => i.Uploader.FirstName.ToLower().Contains(uploaderName.ToLower()) ||
                                                   i.Uploader.LastName.ToLower().Contains(uploaderName.ToLower());
                filters.Add(uploaderNamePredicate);
            }
            if (!string.IsNullOrWhiteSpace(animalName))
            {
                Expression<Func<Image, bool>> animalNamePredicate = i => i.Animal.Name.ToLower().Contains(animalName.ToLower());
                filters.Add(animalNamePredicate);
            }
            if (animalType != null)
            {
                Expression<Func<Image, bool>> animalTypePredicate = i => i.Animal.Type == animalType;
                filters.Add(animalTypePredicate);
            }
            if (takenBefore != null)
            {
                Expression<Func<Image, bool>> takenBeforePredicate = i => i.DateTaken < takenBefore;
                filters.Add(takenBeforePredicate);
            }
            if (takenAfter != null)
            {
                Expression<Func<Image, bool>> takenAfterPredicate = i => i.DateTaken >= takenAfter;
                filters.Add(takenAfterPredicate);
            }

            return await GetPagedAndFiltered<TResult>(queryParameters, filters, "Animal");
        }

        private string CreateImageFilePath(CreateImageDTO imageDTO)
        {
            string uniqueFileName = GetUniqueFileName(imageDTO.Image.FileName);
            
            string filePath = Path.Combine("Images", "Animals", imageDTO.AnimalId.ToString(), uniqueFileName);

            //string pathFromContentRootFolder = Path.Combine(GetPathOfContentRootFolder(), "Animal_Adoption_Management_System_Images\\Animals", imageDTO.AnimalId.ToString(), uniqueFileName);
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

        private string GetPathOfContentRootFolder()
        {
            string contentRootPath = _env.ContentRootPath;
            return contentRootPath
                .Substring(0, contentRootPath
                    .Substring(0, contentRootPath
                        .Substring(0, contentRootPath.LastIndexOf("\\")).LastIndexOf("\\")).LastIndexOf("\\"));
        }
    }
}
