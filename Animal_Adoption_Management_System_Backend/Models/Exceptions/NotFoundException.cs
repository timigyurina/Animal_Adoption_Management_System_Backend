using System.Net;

namespace Animal_Adoption_Management_System_Backend.Models.Exceptions
{
    public class NotFoundException : IException
    {
        // name is for where the key was not found and the value is for the value
        public NotFoundException(string name, object key) : base($"{name} with id ({key}) was not found")
        {
        }

        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
        public override string ErrorType => "Not Found";
    }
}
