namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IUnitOfWork 
    {
        public IAnimalService AnimalService { get; }
        public IShelterService ShelterService { get; }
        public IAnimalShelterService AnimalShelterService { get; }
        public IAnimalBreedService AnimalBreedService { get; }
        public IDonationService DonationService { get; }
        public IImageService ImageService { get; }
        public IUserService UserService { get; }

        public IAdoptionApplicationService AdoptionApplicationService { get; }
        public IAdoptionContractService AdoptionContractService { get; }
        public IManagedAdoptionContractService ManagedAdoptionContractService { get; }
    }
}
