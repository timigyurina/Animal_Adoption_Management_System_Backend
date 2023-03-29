namespace Animal_Adoption_Management_System_Backend.Models.Exceptions
{
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
