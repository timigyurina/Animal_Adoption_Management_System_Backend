﻿using Animal_Adoption_Management_System_Backend.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.DTOs.AnimalBreed
{
    public abstract class BaseAnimalBreedDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        public string? Description { get; set; }
    }
}
