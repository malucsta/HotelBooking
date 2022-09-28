namespace Application
{
    public enum ErrorCode
    {
        // 1 to 99 = guest errors
        NOT_FOUND = 1,
        COULD_NOT_STORE_DATA = 2,
        INVALID_PERSON_ID = 3,
        MISSING_REQUIRED_INFORMATION = 4,
        INVALID_FIELD = 5,

        // 101 to 199 = room errors 
        ROOM_COULD_NOT_STORE_DATA = 102,
        ROOM_MISSING_REQUIRED_INFORMATION = 104,
        ROOM_INVALID_FIELD = 5,
    }

    public abstract class Response
    {
        public bool Sucess { get; set; }
        public string Message { get; set; }
        public ErrorCode ErrorCode { get; set; }

    }
}
