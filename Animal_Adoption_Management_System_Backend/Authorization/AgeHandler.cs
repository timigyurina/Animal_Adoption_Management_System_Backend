using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Animal_Adoption_Management_System_Backend.Authorization
{
    public class AgeHandler : AuthorizationHandler<AgeRequirement>
    {

        private readonly IConfiguration _configuration;
        public AgeHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement requirement)
        {
            bool adminRoleClaim = context.User.IsInRole("Administrator"); 
            if (adminRoleClaim)
                context.Succeed(requirement);

            Claim? dateOfBirthClaim = context.User.FindFirst(
                c => c.Type == ClaimTypes.DateOfBirth && c.Issuer == _configuration["JwtSettings:Issuer"]);

            if (dateOfBirthClaim is null)
                return Task.CompletedTask;

            DateTime dateOfBirth = Convert.ToDateTime(dateOfBirthClaim.Value);

            DateTime dateOfAgeRequirementFulfilment = dateOfBirth.AddYears(requirement.MinimumAge);

            if (DateTime.Now >= dateOfAgeRequirementFulfilment)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
