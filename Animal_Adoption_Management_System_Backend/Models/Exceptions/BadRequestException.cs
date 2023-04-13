using System.Net;

namespace Animal_Adoption_Management_System_Backend.Models.Exceptions
{
    public class BadRequestException :  IException
    {
        public BadRequestException(string? message) : base(message)
        {
        }

        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        public override string ErrorType => "Bad Request";

    }
}
