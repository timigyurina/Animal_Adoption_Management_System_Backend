﻿using Animal_Adoption_Management_System_Backend.Models.Entities;
using System.Security.Claims;

namespace Animal_Adoption_Management_System_Backend.Authorization
{
    public interface IPermissionChecker
    {
        bool CheckPermissionForShelter(int id, ClaimsPrincipal user);
        void CheckPermissionForAdoptionApplication(AdoptionApplication adoptionApplicationWithDetails, ClaimsPrincipal user);
        void CheckPermissionForAdoptionContract(AdoptionContract adoptionContractWithDetails, ClaimsPrincipal user);
        void CheckPermissionForAnimal(Animal animalWithDetails, ClaimsPrincipal user);
        void CheckPermissionForUserRelatedEntity(string userIdOfRelatedEntity, ClaimsPrincipal user, string entityType);
        void CheckPermissionForDonation(Donation donationWithDetails, ClaimsPrincipal user);
        void CheckPermissionForImage(Image imageWithDetails, ClaimsPrincipal user);
    }
}
