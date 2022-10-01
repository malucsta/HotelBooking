namespace Domain.Booking.Exceptions
{
    public class InvalidPeriodException : Exception
    {
        public InvalidPeriodException(string message): base(message) { }
    }
}
