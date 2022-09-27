namespace Domain.DomainExceptions
{
    public class InvalidFieldException : Exception
    {
        public InvalidFieldException(string message) : base(message) { }
    }
}
