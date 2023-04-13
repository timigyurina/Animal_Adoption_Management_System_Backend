using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace Animal_Adoption_Management_System_Backend.Data
{
    public class DataInitialiser
    {
        private readonly AnimalAdoptionContext _context;
        private readonly UserManager<User> _userManager;

        public DataInitialiser(AnimalAdoptionContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            await SeedSheltersAndEmployeesAsync();
            await SeedAnimalBreedsAsync();
            await SeedAnimalsAsync();
        }
        private async Task SeedSheltersAndEmployeesAsync()
        {
            if (!_context.Shelters.Any() && _context.Roles.FirstOrDefault(r => r.Name == "ShelterEmployee") != null)
            {
                Shelter shelter1 = new()
                {
                    Name = "Happy Tails Shelter",
                    Phone = "(555) 123-4567",
                    Email = "happytails@example.com",
                    IsActive = true,
                    Address = new Address
                    {
                        PostalCode = "12345",
                        Country = "United States",
                        Region = "New York",
                        City = "New York City",
                        AddressLineOne = "123 Main Street",
                        AddressLineTwo = "Apt 4B"
                    }
                };
                Shelter shelter2 = new()
                {
                    Name = "Paws and Claws Rescue",
                    Phone = "(555) 987-6543",
                    Email = "pawsandclaws@example.com",
                    IsActive = true,
                    Address = new Address
                    {
                        PostalCode = "V6B 4Y8",
                        Country = "Canada",
                        Region = "British Columbia",
                        City = "Vancouver",
                        AddressLineOne = "456 Granville Street",
                        AddressLineTwo = "Suite 200"
                    }
                };
                Shelter shelter3 = new()
                {
                    Name = "Furry Friends Adoption Center",
                    Phone = "(555) 555-5555",
                    Email = "furryfriends@example.com",
                    IsActive = true,
                    Address = new Address
                    {
                        PostalCode = "WC2H 9JQ",
                        Country = "United Kingdom",
                        Region = "",
                        City = "London",
                        AddressLineOne = "10 Downing Street",
                        AddressLineTwo = "Apt 5"
                    }
                };

                await _context.Shelters.AddRangeAsync(shelter1, shelter2, shelter3);
                await _context.SaveChangesAsync();


                User employee1 = new()
                {
                    Email = "employee@happytails.com",
                    NormalizedEmail = "EMPLOYEE@HAPPYTAILS.COM",
                    UserName = "employee@happytails.com",
                    NormalizedUserName = "EMPLOYEE@HAPPYTAILS.COM",
                    EmailConfirmed = true,
                    FirstName = "Employee",
                    LastName = "Happytails",
                    IsActive = true,
                    DateOfBirth = new DateTime(1980, 2, 2),
                    IsContactOfShelter = true,
                    Shelter = shelter1,
                };
                User employee2 = new()
                {
                    Email = "employee@pawsandclaws.com",
                    NormalizedEmail = "EMPLOYEE@PAWSANDCLAWS.COM",
                    UserName = "employee@pawsandclaws.com",
                    NormalizedUserName = "EMPLOYEE@PAWSANDCLAWS.COM",
                    EmailConfirmed = true,
                    FirstName = "Employee",
                    LastName = "Pawsandclaws",
                    IsActive = true,
                    DateOfBirth = new DateTime(1985, 3, 3),
                    IsContactOfShelter = true,
                    Shelter = shelter2
                };
                User employee3 = new()
                {
                    Email = "employee@furryfriends.com",
                    NormalizedEmail = "EMPLOYEE@HAPPYTAILS.COM",
                    UserName = "employee@furryfriends.com",
                    NormalizedUserName = "EMPLOYEE@FURRYFRIENDS.COM",
                    EmailConfirmed = true,
                    FirstName = "Employee",
                    LastName = "Furryfriends",
                    IsActive = true,
                    DateOfBirth = new DateTime(1990, 4, 4),
                    IsContactOfShelter = true,
                    Shelter = shelter3
                };

                PasswordHasher<User> hasher = new();
                employee1.PasswordHash = hasher.HashPassword(employee1, "Test1.");
                employee2.PasswordHash = hasher.HashPassword(employee2, "Test1.");
                employee3.PasswordHash = hasher.HashPassword(employee3, "Test1.");

                await _userManager.CreateAsync(employee1);
                await _context.SaveChangesAsync();
                await _userManager.CreateAsync(employee2);
                await _context.SaveChangesAsync();
                await _userManager.CreateAsync(employee3);
                await _context.SaveChangesAsync();

                await _userManager.AddToRoleAsync(employee1, "ShelterEmployee");
                await _userManager.AddToRoleAsync(employee2, "ShelterEmployee");
                await _userManager.AddToRoleAsync(employee3, "ShelterEmployee");

                await _context.SaveChangesAsync();
            }

        }
        private async Task SeedAnimalBreedsAsync()
        {

            if (!_context.AnimalBreeds.Any() && _context.Users.Any())
            {
                AnimalBreed[] animalBreeds = new AnimalBreed[]
                {
                    new AnimalBreed
                    {
                        Name = "Golden Retriever",
                        Description = "Friendly and intelligent, a great family pet",
                        Type = AnimalType.Dog
                    },
                    new AnimalBreed
                    {
                        Name = "Persian",
                        Description = "Long-haired and docile, a popular housecat",
                        Type = AnimalType.Cat
                    },
                    new AnimalBreed
                    {
                        Name = "Poodle",
                        Description = "Hypoallergenic and loyal, a great companion dog",
                        Type = AnimalType.Dog
                    },
                    new AnimalBreed
                    {
                        Name = "Sphynx",
                        Description = "Hairless and playful, a unique and attention-grabbing cat breed",
                        Type = AnimalType.Cat
                    },
                    new AnimalBreed
                    {
                        Name = "Labrador Retriever",
                        Description = "A friendly and energetic breed, great for families with children",
                        Type = AnimalType.Dog
                    },
                    new AnimalBreed
                    {                       
                        Name = "Siamese",
                        Description = "A vocal and intelligent breed, known for their striking blue eyes",
                        Type = AnimalType.Cat
                    },
                    new AnimalBreed
                    {                       
                        Name = "German Shepherd",
                        Description = "A loyal and protective breed, often used in law enforcement and military work",
                        Type = AnimalType.Dog
                    },
                    new AnimalBreed
                    {                       
                        Name = "Bengal",
                        Description = "A highly active and playful breed, often compared to a small leopard",
                        Type = AnimalType.Cat
                    },
                    new AnimalBreed
                    {                       
                        Name = "Chihuahua",
                        Description = "A tiny and spunky breed, often referred to as a 'purse dog'",
                        Type = AnimalType.Dog
                    },
                    new AnimalBreed
                    {                       
                        Name = "Ragdoll",
                        Description = "A docile and gentle breed of cat",
                        Type = AnimalType.Cat
                    }
                };
                await _context.AnimalBreeds.AddRangeAsync(animalBreeds);
                await _context.SaveChangesAsync();

            }
        }
        private async Task SeedAnimalsAsync()
        {
            if (!_context.Animals.Any() && !_context.AnimalShelters.Any() && _context.AnimalBreeds.Count() > 9 && _context.Shelters.Count() > 2)
            {
                Animal animal1 = new()
                {
                    Name = "Max",
                    Type = AnimalType.Dog,
                    Size = AnimalSize.Medium,
                    Status = AnimalStatus.Adopted,
                    Gender = Gender.Male,
                    IsSterilised = true,
                    SterilisationDate = new DateTime(2017, 5, 12),
                    BirthDate = new DateTime(2015, 1, 1),
                    Color = AnimalColor.Brown,
                    Notes = "Loves to play fetch!",
                    Breed = _context.AnimalBreeds.First(b => b.Type == AnimalType.Dog),
                };
                Animal animal2 = new()
                {
                    Name = "Luna",
                    Type = AnimalType.Cat,
                    Size = AnimalSize.Small,
                    Status = AnimalStatus.WaitingForAdoption,
                    Gender = Gender.Female,
                    IsSterilised = false,
                    SterilisationDate = null,
                    BirthDate = new DateTime(2020, 3, 15),
                    Color = AnimalColor.Gray,
                    Notes = "Loves to cuddle and play with toys!",
                    Breed = _context.AnimalBreeds.First(b => b.Type == AnimalType.Cat)
                };
                Animal animal3 = new()
                {
                    Name = "Rocky",
                    Type = AnimalType.Dog,
                    Size = AnimalSize.Large,
                    Status = AnimalStatus.NotAdoptable,
                    Gender = Gender.Male,
                    IsSterilised = true,
                    SterilisationDate = new DateTime(2014, 11, 30),
                    BirthDate = new DateTime(2012, 9, 3),
                    Color = AnimalColor.Black,
                    Notes = "Loves to run and play fetch!",
                    Breed = _context.AnimalBreeds.First(b => b.Type == AnimalType.Dog)
                };
                Animal animal4 = new()
                {
                    Name = "Daisy",
                    Type = AnimalType.Dog,
                    Size = AnimalSize.Small,
                    Status = AnimalStatus.WaitingForAdoption,
                    Gender = Gender.Female,
                    IsSterilised = true,
                    SterilisationDate = new DateTime(2018, 4, 10),
                    BirthDate = new DateTime(2016, 12, 25),
                    Color = AnimalColor.Yellow,
                    Notes = "Loyal and obedient, knows many tricks!",
                    Breed = _context.AnimalBreeds.First(b => b.Type == AnimalType.Dog)
                };
                Animal animal5 = new()
                {
                    Name = "Whiskers",
                    Type = AnimalType.Cat,
                    Size = AnimalSize.Medium,
                    Status = AnimalStatus.WaitingForAdoption,
                    Gender = Gender.Other,
                    IsSterilised = false,
                    SterilisationDate = null,
                    BirthDate = new DateTime(2019, 6, 1),
                    Color = AnimalColor.Calico,
                    Notes = "Very playful and loves to climb!",
                    Breed = _context.AnimalBreeds.First(b => b.Type == AnimalType.Cat)
                };
                Animal animal6 = new()
                {
                    Name = "Cooper",
                    Type = AnimalType.Dog,
                    Size = AnimalSize.Medium,
                    Status = AnimalStatus.WaitingForAdoption,
                    Gender = Gender.Male,
                    IsSterilised = true,
                    SterilisationDate = new DateTime(2021, 8, 20),
                    BirthDate = new DateTime(2020, 4, 4),
                    Color = AnimalColor.Black,
                    Notes = "Friendly and loves to play with other dogs!",
                    Breed = _context.AnimalBreeds.First(b => b.Type == AnimalType.Dog)
                };
                Animal animal7 = new()
                {
                    Name = "Buddy",
                    Type = AnimalType.Cat,
                    Size = AnimalSize.Small,
                    Status = AnimalStatus.Adopted,
                    Gender = Gender.Female,
                    IsSterilised = true,
                    SterilisationDate = new DateTime(2019, 5, 3),
                    BirthDate = new DateTime(2018, 1, 14),
                    Color = AnimalColor.Striped,
                    Notes = "Loves to play with strings and balls!",
                    Breed = _context.AnimalBreeds.First(b => b.Type == AnimalType.Cat)
                };
                Animal animal8 = new()
                {
                    Name = "Oliver",
                    Type = AnimalType.Cat,
                    Size = AnimalSize.Small,
                    Status = AnimalStatus.WaitingForAdoption,
                    Gender = Gender.Male,
                    IsSterilised = true,
                    SterilisationDate = new DateTime(2019, 4, 25),
                    BirthDate = new DateTime(2017, 8, 3),
                    Color = AnimalColor.Gray,
                    Notes = "Independent and likes to take naps!",
                    Breed = _context.AnimalBreeds.First(b => b.Type == AnimalType.Cat)
                };
                Animal animal9 = new()
                {
                    Name = "Bella",
                    Type = AnimalType.Dog,
                    Size = AnimalSize.Large,
                    Status = AnimalStatus.WaitingForAdoption,
                    Gender = Gender.Female,
                    IsSterilised = false,
                    SterilisationDate = null,
                    BirthDate = new DateTime(2022, 2, 14),
                    Color = AnimalColor.Red,
                    Notes = "Very energetic and loves to run!",
                    Breed = _context.AnimalBreeds.First(b => b.Type == AnimalType.Dog)
                };

                await _context.Animals.AddRangeAsync(animal1, animal2, animal3, animal4, animal5, animal6, animal7, animal8, animal9);
                await _context.SaveChangesAsync();

                Shelter shelter1 = _context.Shelters.First();
                Shelter shelter2 = _context.Shelters.Skip(1).First();
                Shelter shelter3 = _context.Shelters.Skip(2).First();

                AnimalShelter[] animalShelters = new AnimalShelter[]
                {
                    new AnimalShelter
                    {
                        Animal = animal1,
                        Shelter = shelter1,
                        EnrollmentDate = animal1.BirthDate.AddMonths(2),
                        ExitDate = animal1.BirthDate.AddMonths(5)
                    },
                    new AnimalShelter
                    {
                        Animal = animal2,
                        Shelter = shelter1,
                        EnrollmentDate = animal2.BirthDate.AddMonths(2),
                        ExitDate = null
                    },
                    new AnimalShelter
                    {
                        Animal = animal3,
                        Shelter = shelter1,
                        EnrollmentDate = animal3.BirthDate.AddMonths(2),
                        ExitDate = null
                    },
                    new AnimalShelter
                    {
                        Animal = animal4,
                        Shelter = shelter2,
                        EnrollmentDate = animal1.BirthDate.AddMonths(2),
                        ExitDate = null
                    },
                    new AnimalShelter
                    {
                        Animal = animal5,
                        Shelter = shelter2,
                        EnrollmentDate = animal5.BirthDate.AddMonths(2),
                        ExitDate = null
                    },
                    new AnimalShelter
                    {
                        Animal = animal6,
                        Shelter = shelter2,
                        EnrollmentDate = animal6.BirthDate.AddMonths(2),
                        ExitDate = null
                    },
                    new AnimalShelter
                    {
                        Animal = animal7,
                        Shelter = shelter3,
                        EnrollmentDate = animal7.BirthDate.AddMonths(2),
                        ExitDate = animal7.BirthDate.AddMonths(1)
                    },
                    new AnimalShelter
                    {
                        Animal = animal8,
                        Shelter = shelter3,
                        EnrollmentDate = animal8.BirthDate.AddMonths(2),
                        ExitDate = null
                    },
                    new AnimalShelter
                    {
                        Animal = animal9,
                        Shelter = shelter3,
                        EnrollmentDate = animal9.BirthDate.AddMonths(2),
                        ExitDate = animal9.BirthDate.AddMonths(4)
                    },
                    new AnimalShelter
                    {
                        Animal = animal9,
                        Shelter = shelter2,
                        EnrollmentDate = animal9.BirthDate.AddMonths(4),
                        ExitDate = null
                    }
                };

                await _context.AnimalShelters.AddRangeAsync(animalShelters);
                await _context.SaveChangesAsync();
            }
        }
    }
}
    
