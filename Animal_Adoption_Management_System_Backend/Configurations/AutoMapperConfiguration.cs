using Animal_Adoption_Management_System_Backend.Models.DTOs.AddressDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionApplicationDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AdoptionContractDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalBreedDTOS;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalShelterDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.DonationDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ManagedAdoptionContractDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.ShelterDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.UserAuthDTOs;
using Animal_Adoption_Management_System_Backend.Models.DTOs.UserDTOs;
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
            CreateMap<Animal, AnimalDTOWithBreed>().ReverseMap();
            CreateMap<Animal, AnimalDTOWithInfoForAdopters>().ReverseMap();
            CreateMap<Animal, UpdateAnimalDTO>().ReverseMap();
            
            CreateMap<Shelter, CreateShelterDTO>().ReverseMap();
            CreateMap<Shelter, ShelterDTO>().ReverseMap();
            CreateMap<Shelter, ShelterDTOWithDetails>().ReverseMap();
            CreateMap<Shelter, ShelterDTOWithAddress>().ReverseMap();
            CreateMap<Shelter, UpdateShelterContactInfoDTO>().ReverseMap();

            CreateMap<AnimalShelter, AnimalShelterDTO>().ReverseMap();
            
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<Address, CreateAddressDTO>().ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserDTOWithDetails>().ReverseMap();
            CreateMap<User, LoginUserDTO>().ReverseMap();
            CreateMap<User, RegisterUserDTO>().ReverseMap();

            CreateMap<Donation, DonationDTO>().ReverseMap();
            CreateMap<Donation, DonationDTOWithDetails>().ReverseMap();
            CreateMap<Donation, CreateDonationDTO>().ReverseMap();

            CreateMap<AdoptionApplication, AdoptionApplicationDTO>().ReverseMap();
            CreateMap<AdoptionApplication, AdoptionApplicationDTOWithDetails>().ReverseMap();
            CreateMap<AdoptionApplication, CreateAdoptionApplicationDTO>().ReverseMap();

            CreateMap<AdoptionContract, AdoptionContractDTO>().ReverseMap();
            CreateMap<AdoptionContract, AdoptionContractDTOWithDetails>().ReverseMap();
            CreateMap<AdoptionContract, CreateAdoptionContractDTO>().ReverseMap();
            CreateMap<AdoptionContract, AdoptionContractDTOWithManagerDetails>().ReverseMap();

            CreateMap<ManagedAdoptionContract, ManagedAdoptionContractDTOWithDetails>().ReverseMap();

            CreateMap<Image, ImageDTO>().ReverseMap();
            CreateMap<Image, ImageDTOWithDetails>().ReverseMap();
            CreateMap<Image, ImageDTOWithAnimal>().ReverseMap();
            CreateMap<Image, CreateImageDTO>().ReverseMap();
        }
    }
}
