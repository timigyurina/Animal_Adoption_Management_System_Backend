using Microsoft.AspNetCore.Authorization;

namespace Animal_Adoption_Management_System_Backend.Authorization
{
    public class AgeRequirement : IAuthorizationRequirement
    {
        public AgeRequirement(int minimumAge)
        {
            MinimumAge = minimumAge;
        }
        public int MinimumAge { get; }
    }
}
