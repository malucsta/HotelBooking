namespace Domain.DomainExceptions
{
    public class InvalidFieldException : Exception
    {
        public InvalidFieldException() { }
        public InvalidFieldException(string message) : base(message) { }
    }
}
