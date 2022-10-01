using Application.Guest.DTOs;
using Application.Room.DTOs;
using Entities = Domain.Entities;
using Domain.Enums;

namespace Application.Booking.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int RoomId { get; set; }
        public int GuestId { get; set; }

        public BookingDTO()
        {
            this.PlacedAt = DateTime.UtcNow;
        }

        public static Entities.Booking MapToEntity(BookingDTO bookingDTO)
        {
            return new Entities.Booking
            {
                Id = bookingDTO.Id,
                PlacedAt = bookingDTO.PlacedAt,
                Start = bookingDTO.Start,
                End = bookingDTO.End,
                Room = new Entities.Room { Id = bookingDTO.RoomId },
                Guest = new Entities.Guest { Id = bookingDTO.GuestId },
            };
        }

        public static BookingDTO MapToDTO(Entities.Booking booking)
        {
            return new BookingDTO
            {
                Id = booking.Id,
                PlacedAt = booking.PlacedAt,
                Start = booking.Start,
                End = booking.End,
                RoomId = booking.Room.Id,
                GuestId = booking.Guest.Id,
            };
        }
    }
}
