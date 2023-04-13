using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Services.Implementations;
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
            _logger.LogWarning($"Evaluating authorization requirement for Shelter with id {id}");

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
                    _logger.LogInformation("Employee's ShelterId claim equals requested Shelter's id");
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

        public void CheckPermissionForAnimal(Animal animalWithDetails, ClaimsPrincipal user)
        {
            _logger.LogWarning($"Evaluating authorization requirement for Animal with id {animalWithDetails.Id}");

            if (user.IsInRole("Administrator"))
            {
                _logger.LogInformation("Admin request for Animal");
                return;
            }

            Claim? shelterIdClaim = user.Claims.FirstOrDefault(c => c.Type == "ShelterId");

            if (shelterIdClaim != null)
            {
                int idOfLatestShelter = GetLatestShelterIdOfAnimal(animalWithDetails);
                if (shelterIdClaim.Value == idOfLatestShelter.ToString())
                {
                    _logger.LogInformation("Employee's ShelterId claim equals requested Animal's ShelterId");
                    return;
                }
                else
                {
                    _logger.LogInformation($"Current user's ShelterId claim ({shelterIdClaim.Value}) does not satisfy the ShelterId authorization requirement {idOfLatestShelter}");
                }
            }
            else
            {
                _logger.LogInformation("No ShelterId claim present");
            }
            throw new ForbiddenException($"Animal");
        }

        public void CheckPermissionForAdoptionApplication(AdoptionApplication adoptionApplicationWithDetails, ClaimsPrincipal user)
        {
            _logger.LogWarning($"Evaluating authorization requirement for AdoptionApplication with id {adoptionApplicationWithDetails.Id}");

            if (user.IsInRole("Administrator"))
            {
                _logger.LogInformation("Admin request for AdoptionApplication");
                return;
            }

            Claim? identityClaim = user.Claims.FirstOrDefault(c => c.Type == "UserId");
            Claim? shelterIdClaim = user.Claims.FirstOrDefault(c => c.Type == "ShelterId");
            if (identityClaim != null)
            {
                bool isOwner = HandlePermissionIfOwnerOfEntity(adoptionApplicationWithDetails.Applier.Id, identityClaim.Value, "AdoptionApplication");
                if (isOwner) return;
            }
            if (shelterIdClaim != null)
            {
                int shelterIdOfAnimalInAdoptionApplication = GetLatestShelterIdOfAnimal(adoptionApplicationWithDetails.Animal);
                bool isAuthorizedEmployee = HandlePermissionIfEmployee(shelterIdOfAnimalInAdoptionApplication, shelterIdClaim.Value, "AdoptionApplication");
                if (isAuthorizedEmployee) return;
            }
            throw new ForbiddenException($"AdoptionApplication");
        }

        public void CheckPermissionForAdoptionContract(AdoptionContract adoptionContractWithDetails, ClaimsPrincipal user)
        {
            _logger.LogWarning($"Evaluating authorization requirement for AdoptionContract with id {adoptionContractWithDetails.Id}");

            if (user.IsInRole("Administrator"))
            {
                _logger.LogInformation("Admin request for AdoptionContract");
                return;
            }

            Claim? identityClaim = user.Claims.FirstOrDefault(c => c.Type == "UserId");
            Claim? shelterIdClaim = user.Claims.FirstOrDefault(c => c.Type == "ShelterId");
            if (identityClaim != null)
            {
                bool isOwner = HandlePermissionIfOwnerOfEntity(adoptionContractWithDetails.Applier.Id, identityClaim.Value, "AdoptionContract");
                if (isOwner) return;
            }
            if (shelterIdClaim != null)
            {
                int shelterIdOfAnimalInAdoptionContract = GetLatestShelterIdOfAnimal(adoptionContractWithDetails.Animal);
                bool isAuthorizedEmployee = HandlePermissionIfEmployee(shelterIdOfAnimalInAdoptionContract, shelterIdClaim.Value, "AdoptionContract");
                if (isAuthorizedEmployee) return;
            }
            throw new ForbiddenException($"AdoptionContract");
        }

        public void CheckPermissionForUserRelatedEntity(string userIdOfRelatedEntity, ClaimsPrincipal user, string entityType)
        {
            _logger.LogWarning($"Evaluating authorization requirement for User-related {entityType} entity");

            if (user.IsInRole("Administrator"))
            {
                _logger.LogInformation($"Admin request for {entityType}, approved");
                return;
            }

            Claim? identityClaim = user.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (identityClaim != null)
            {
                bool isOwner = HandlePermissionIfOwnerOfEntity(userIdOfRelatedEntity, identityClaim.Value, entityType);
                if (isOwner) return;
            }
            else
                _logger.LogInformation("No UserId claim present");

            throw new ForbiddenException($"other User's {entityType}(s)");
        }

        public void CheckPermissionForDonation(Donation donationWithDetails, ClaimsPrincipal user)
        {
            _logger.LogWarning($"Evaluating authorization requirement for Donation with id {donationWithDetails.Id}");

            if (user.IsInRole("Administrator"))
            {
                _logger.LogInformation("Admin request for Donation");
                return;
            }

            Claim? identityClaim = user.Claims.FirstOrDefault(c => c.Type == "UserId");
            Claim? shelterIdClaim = user.Claims.FirstOrDefault(c => c.Type == "ShelterId");
            if (identityClaim != null)
            {
                bool isOwner = HandlePermissionIfOwnerOfEntity(donationWithDetails.Donator.Id, identityClaim.Value, "Donation");
                if (isOwner) return;
            }
            if (shelterIdClaim != null)
            {
                bool isAuthorizedEmployee = HandlePermissionIfEmployee(donationWithDetails.Shelter.Id, shelterIdClaim.Value, "Donation");
                if (isAuthorizedEmployee) return;
            }
            throw new ForbiddenException($"Donation");
        }

        public void CheckPermissionForImage(Image imageWithDetails, ClaimsPrincipal user)
        {
            _logger.LogWarning($"Evaluating authorization requirement for Image with id {imageWithDetails.Id}");

            if (user.IsInRole("Administrator"))
            {
                _logger.LogInformation("Admin request for Image");
                return;
            }

            Claim? shelterIdClaim = user.Claims.FirstOrDefault(c => c.Type == "ShelterId");
            if (shelterIdClaim != null)
            {
                int shelterIdOfAnimalInImage = GetLatestShelterIdOfAnimal(imageWithDetails.Animal);
                bool isAuthorizedEmployee = HandlePermissionIfEmployee(shelterIdOfAnimalInImage, shelterIdClaim.Value, "Image");
                if (isAuthorizedEmployee) return;
            }
            throw new ForbiddenException($"Image");
        }


        private bool HandlePermissionIfEmployee(int IdToCheck, string shelterIdClaimValue, string entityType)
        {
            _logger.LogWarning($"Checking if Employee's ShelterId claim equals the ShelterId of requested {entityType}");
            if (shelterIdClaimValue == IdToCheck.ToString())
            {
                _logger.LogInformation($"Employee authorized for requested {entityType}");
                return true;
            }
            _logger.LogInformation($"Current Employee's ShelterId claim ({shelterIdClaimValue}) does not equal the ShelterId of this {entityType} ({IdToCheck})");
            return false;
        }
        private bool HandlePermissionIfOwnerOfEntity(string idToCheck, string identityClaimValue, string entityType)
        {
            _logger.LogWarning($"Checking if the requested {entityType} belongs to User");
            if (idToCheck == identityClaimValue)
            {
                _logger.LogInformation($"User's identity claim does not authorize viewing the requested {entityType}");
                return true;
            }
            _logger.LogInformation($"Current {entityType} does not belong to User ({idToCheck})");
            return false;
        }

        private int GetLatestShelterIdOfAnimal(Animal animalWithDetails)
        {
            int shelterId;
            AnimalShelter? currentAnimalShelterConnection = animalWithDetails.AnimalShelters.FirstOrDefault(animalS => animalS.ExitDate == null);
            if (currentAnimalShelterConnection == null)
                shelterId = animalWithDetails.AnimalShelters
                    .Aggregate((latest, curr) => latest.ExitDate > curr.ExitDate ? latest : curr).Shelter.Id;
            else
                shelterId = currentAnimalShelterConnection.Shelter.Id;

            return shelterId;
        }
    }
}
