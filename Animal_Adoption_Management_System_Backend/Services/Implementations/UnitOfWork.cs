using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AnimalAdoptionContext _context;

        private readonly IAnimalService _animalService;
        private readonly IShelterService _shelterService;
        private readonly IAnimalShelterService _animalShelterService;
        private readonly IAnimalBreedService _animalBreedService;
        private readonly IDonationService _donationService;
        private readonly IImageService _imageService;
        private readonly IUserService _userService;

        private readonly IAdoptionApplicationService _adoptionApplicationService;
        private readonly IAdoptionContractService _adoptionContractService;
        private readonly IManagedAdoptionContractService _managedAdoptionContractService;

        public UnitOfWork(
            AnimalAdoptionContext context,
            IAnimalService animalService,
            IShelterService shelterService,
            IAnimalShelterService animalShelterService,
            IAnimalBreedService animalBreedService,
            IDonationService donationService,
            IImageService imageService,
            IAdoptionApplicationService adoptionApplicationService,
            IAdoptionContractService adoptionContractService,
            IManagedAdoptionContractService managedAdoptionContractService,
            IUserService userService)
        {
            _context = context;
            _animalService = animalService;
            _shelterService = shelterService;
            _animalShelterService = animalShelterService;
            _animalBreedService = animalBreedService;
            _donationService = donationService;
            _imageService = imageService;
            _adoptionApplicationService = adoptionApplicationService;
            _adoptionContractService = adoptionContractService;
            _managedAdoptionContractService = managedAdoptionContractService;
            _userService = userService;
        }


        public IAnimalService AnimalService => _animalService;
        public IShelterService ShelterService => _shelterService;
        public IAnimalShelterService AnimalShelterService => _animalShelterService;
        public IAnimalBreedService AnimalBreedService => _animalBreedService;
        public IDonationService DonationService => _donationService;
        public IImageService ImageService => _imageService;
        public IUserService UserService => _userService;
        public IAdoptionApplicationService AdoptionApplicationService => _adoptionApplicationService;
        public IAdoptionContractService AdoptionContractService => _adoptionContractService;
        public IManagedAdoptionContractService ManagedAdoptionContractService => _managedAdoptionContractService;
    }
}
