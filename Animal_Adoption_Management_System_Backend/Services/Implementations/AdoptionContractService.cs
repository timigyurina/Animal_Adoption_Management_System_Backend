﻿using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Repositories;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class AdoptionContractService : GenericRepository<AdoptionContract>, IAdoptionContractService
    {
        public AdoptionContractService(AnimalAdoptionContext context) : base(context)
        {
        }
    }
}
