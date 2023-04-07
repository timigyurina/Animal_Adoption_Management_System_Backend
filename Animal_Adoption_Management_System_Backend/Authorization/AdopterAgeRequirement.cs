using Microsoft.AspNetCore.Authorization;

namespace Animal_Adoption_Management_System_Backend.Authorization
{
    public class AdopterAgeRequirement : IAuthorizationRequirement
    {
        public AdopterAgeRequirement(int minimumAge)
        {
            MinimumAge = minimumAge;
        }
        public int MinimumAge { get; }
    }
}
