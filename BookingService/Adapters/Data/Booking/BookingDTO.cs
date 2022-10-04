using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities = Domain.Entities; 

namespace Data.Booking
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int RoomId { get; set; }
        public int GuestId { get; set; }

        public static Entities.Booking MapToEntity(BookingDTO bookingDTO)
        {
            return new Entities.Booking
            {
                Id = bookingDTO.Id,
                PlacedAt = bookingDTO.PlacedAt,
                Start = bookingDTO.Start,
                End = bookingDTO.End,
                Room = new Entities.Room
                {
                    Id = bookingDTO.RoomId,
                },
                Guest = new Entities.Guest
                {
                    Id = bookingDTO.GuestId,
                },
            };
        }
    }
}
