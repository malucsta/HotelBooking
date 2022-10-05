namespace Application
{
    public enum ErrorCode
    {
        UNKNOWN_ERROR = 0,

        // 1 to 99 = guest errors
        NOT_FOUND = 1,
        COULD_NOT_STORE_DATA = 2,
        INVALID_PERSON_ID = 3,
        MISSING_REQUIRED_INFORMATION = 4,
        INVALID_FIELD = 5,

        // 101 to 199 = room errors 
        ROOM_NOT_FOUND = 101,
        ROOM_COULD_NOT_STORE_DATA = 102,
        ROOM_MISSING_REQUIRED_INFORMATION = 104,
        ROOM_INVALID_FIELD = 105,

        //201 to 299 = booking errors 
        BOOKING_NOT_FOUND = 201,
        BOOKING_INVALID_FIELD = 205,
        BOOKING_INVALID_GUEST = 206,
        BOOKING_ROOM_NOT_AVAILABLE = 207,
        BOOKING_INVALID_OPERATION = 299,

    }

    public abstract class Response
    {
        public bool Sucess { get; set; }
        public string Message { get; set; }
        public ErrorCode ErrorCode { get; set; }

    }
}
