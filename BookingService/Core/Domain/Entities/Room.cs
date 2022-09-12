namespace Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool InMantainance { get; set; }

        public bool IsAvailable
        {
            get { return !InMantainance && !HasGuests; }
        }

        public bool HasGuests
        {
            //TODO: check if this room has open bookings 
            get { return true; }
        }
    }
}
