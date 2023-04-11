using System.Net;

namespace Animal_Adoption_Management_System_Backend.Models.Exceptions
{
    public abstract class IException : Exception
    {
        protected IException(string? message) : base(message)
        {
        }

        public abstract HttpStatusCode StatusCode { get; }
        public abstract string ErrorType { get; }
        public string Message => base.Message;
    }
}
