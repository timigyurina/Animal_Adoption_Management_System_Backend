using System.Net;

namespace Animal_Adoption_Management_System_Backend.Models.Exceptions
{
    public class ForbiddenException : IException
    {
        public ForbiddenException(object objectOfPermission) : base($"User has no permission to see or modify {objectOfPermission}")
        {
        }

        public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden;
        public override string ErrorType => "Forbidden";
    }
}
