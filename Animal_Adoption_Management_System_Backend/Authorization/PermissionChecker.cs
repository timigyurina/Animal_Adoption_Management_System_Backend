using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Animal_Adoption_Management_System_Backend.Authorization
{
    public class PermissionChecker : IPermissionChecker
    {
        private readonly ILogger<AuthManager> _logger;

        public PermissionChecker(ILogger<AuthManager> logger)
        {
            _logger = logger;
        }

        public void CheckPermissionForShelter(int id, ClaimsPrincipal user)
        {
            _logger.LogWarning($"Evaluating authorization requirement for ShelterId == {id}");

            if (user.IsInRole("Administrator"))
            {
                _logger.LogInformation("Admin request for Shelter");
                return;
            }

            Claim? shelterIdClaim = user.Claims.FirstOrDefault(c => c.Type == "ShelterId");

            if (shelterIdClaim != null)
            {
                if (shelterIdClaim.Value == id.ToString())
                {
                    _logger.LogInformation("Employee's ShelterId claim equals requested ShelterId");
                    return;
                }
                else
                {
                    _logger.LogInformation($"Current user's ShelterId claim ({shelterIdClaim.Value}) does not satisfy the ShelterId authorization requirement {id}");
                }
            }
            else
            {
                _logger.LogInformation("No ShelterId claim present");
            }
            throw new ForbiddenException($"Shelter");
        }

        public void CheckPermissionForAdoptionApplication(AdoptionApplication adoptionApplication, ClaimsPrincipal user)
        {
            _logger.LogWarning($"Evaluating authorization requirement for AdoptionApplication with id {adoptionApplication.Id}");

            if (user.IsInRole("Administrator"))
            {
                _logger.LogInformation("Admin request for AdoptionApplication");
                return;
            }

            Claim? shelterIdClaim = user.Claims.FirstOrDefault(c => c.Type == "ShelterId");
            Claim? identityClaim = user.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

            if (shelterIdClaim != null)
            {
                HandlePermissionIfEmployee(adoptionApplication.Animal, shelterIdClaim.Value);
                return;
            }
            else if (identityClaim != null)
            {
                HandlePermissionIfAdopter(adoptionApplication.Applier.Id, identityClaim.Value);
            }
            throw new ForbiddenException($"AdoptionApplication");
        }

        public void CheckPermissionForAdoptionContract(AdoptionContract adoptionContract, ClaimsPrincipal user)
        {
            _logger.LogWarning($"Evaluating authorization requirement for AdoptionContract with id {adoptionContract.Id}");

            if (user.IsInRole("Administrator"))
            {
                _logger.LogInformation("Admin request for AdoptionContract");
                return;
            }

            Claim? shelterIdClaim = user.Claims.FirstOrDefault(c => c.Type == "ShelterId");
            Claim? identityClaim = user.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

            if (shelterIdClaim != null)
            {
                HandlePermissionIfEmployee(adoptionContract.Animal, shelterIdClaim.Value);
                return;
            }
            else if (identityClaim != null)
            {
                HandlePermissionIfAdopter(adoptionContract.Applier.Id, identityClaim.Value);
            }
            throw new ForbiddenException($"AdoptionContract");
        }

        private void HandlePermissionIfEmployee(Animal animal, string shelterIdClaimValue)
        {
            int shelterIdOfAnimalInAdoptionApplication;
            AnimalShelter? currentAnimalShelterConnection = animal.AnimalShelters.FirstOrDefault(animalS => animalS.ExitDate == null);
            if (currentAnimalShelterConnection == null)
            {
                shelterIdOfAnimalInAdoptionApplication = animal.AnimalShelters
                    .Aggregate((latest, curr) => latest.ExitDate > curr.ExitDate ? latest : curr).Shelter.Id;
            }
            else
            {
                shelterIdOfAnimalInAdoptionApplication = currentAnimalShelterConnection.Shelter.Id;
            }

            if (shelterIdClaimValue == shelterIdOfAnimalInAdoptionApplication.ToString())
            {
                _logger.LogInformation("Employee's ShelterId claim equals the ShelterId of requested AdoptionApplication/AdoptionContract's Animal");
                return;
            }
            else
            {
                _logger.LogInformation($"Current user's ShelterId claim ({shelterIdClaimValue}) does not equal the ShelterId of this AdoptionApplication ({shelterIdOfAnimalInAdoptionApplication})");
                throw new ForbiddenException($"AdoptionApplication/AdoptionContract");
            }
        }
        private void HandlePermissionIfAdopter(string applierId, string identityClaimValue)
        {
            _logger.LogInformation("No ShelterId claim present, checking if User is Applier of requested AdoptionApplication/AdoptionContract");

            if (applierId == identityClaimValue)
            {
                _logger.LogInformation("User's identity claim equals the ApplierId of requested AdoptionApplication/AdoptionContract");
                return;
            }
        }
    }
}
