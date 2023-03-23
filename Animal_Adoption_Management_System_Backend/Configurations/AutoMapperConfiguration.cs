using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalBreedDTOS;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalShelterDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ShelterDTOs;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using AutoMapper;

namespace Animal_Adoption_Management_System_Backend.Configurations
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<AnimalBreed, AnimalBreedDTO>().ReverseMap();
            CreateMap<AnimalBreed, CreateAnimalBreedDTO>().ReverseMap();

            CreateMap<Animal, CreateAnimalDTO>().ReverseMap();
            CreateMap<Animal, AnimalDTO>().ReverseMap();
            CreateMap<Animal, AnimalDTOWithDetails>().ReverseMap();
            CreateMap<Animal, UpdateAnimalDTO>().ReverseMap();
            
            CreateMap<Shelter, CreateShelterDTO>().ReverseMap();
            CreateMap<Shelter, ShelterDTO>().ReverseMap();

            CreateMap<AnimalShelter, AnimalShelterDTO>().ReverseMap();
        }
    }
}
