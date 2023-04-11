using Animal_Adoption_Management_System_Backend.Models.Entities;
using System.Security.Claims;

namespace Animal_Adoption_Management_System_Backend.Authorization
{
    public interface IPermissionChecker
    {
        void CheckPermissionForShelter(int id, ClaimsPrincipal user);
        void CheckPermissionForAdoptionApplication(AdoptionApplication adoptionApplication, ClaimsPrincipal user);
        void CheckPermissionForAdoptionContract(AdoptionContract adoptionContract, ClaimsPrincipal user);
    }
}
