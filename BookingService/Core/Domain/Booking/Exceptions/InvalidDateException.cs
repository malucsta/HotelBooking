namespace Domain.Booking.Exceptions
{
    public class InvalidDateException : Exception
    {
        public InvalidDateException(string message) : base(message) {}
    }
}
