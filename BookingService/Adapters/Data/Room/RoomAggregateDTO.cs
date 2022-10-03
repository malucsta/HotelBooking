using Entities = Domain.Entities;

namespace Data.Room
{
    public class RoomAggregateDTO
    {
        public int Id { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int RoomId { get; set; }
        public int GuestId { get; set; }

        public static Entities.Booking MapToEntity(RoomAggregateDTO aggregateDTO)
        {
            return new Entities.Booking
            {
                Id = aggregateDTO.Id,
                PlacedAt = aggregateDTO.PlacedAt,
                Start = aggregateDTO.Start,
                End = aggregateDTO.End,
                Room = new Entities.Room
                {
                    Id = aggregateDTO.RoomId,
                },
                Guest = new Entities.Guest
                {
                    Id = aggregateDTO.GuestId,
                },
            };
        }
    }
}
