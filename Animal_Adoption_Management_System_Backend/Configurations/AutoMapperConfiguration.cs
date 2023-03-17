using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalBreed;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using AutoMapper;

namespace Animal_Adoption_Management_System_Backend.Configurations
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<AnimalBreed, AnimalBreedDTO>().ReverseMap();
        }
    }
}
