using Application.Booking.DTOs;
using Domain.Enums;
using Domain.ValueObjects;
using Entities = Domain.Entities;


namespace Application.Room.DTOs
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool InMantainance { get; set; }

        public decimal Value { get; set; }

        public AcceptedCurrencies Currency { get; set; }

        public List<BookingDTO>? Bookings { get; set; }

        public static Entities.Room MapToEntity(RoomDTO roomDTO)
        {
            return new Entities.Room
            {
                Id = roomDTO.Id,
                Name = roomDTO.Name,
                Level = roomDTO.Level,
                InMantainance = roomDTO.InMantainance,
                Price = new Domain.ValueObjects.Price
                {
                    Value = roomDTO.Value,
                    Currency = roomDTO.Currency,
                },
            };
        }

        public static RoomDTO MapToDTO(Entities.Room room)
        {
            var bookingsDTOs = new List<BookingDTO>();
            foreach (var booking in room.Bookings)
            {
                bookingsDTOs.Add(BookingDTO.MapToDTO(booking));
            }

            return new RoomDTO
            {
                Id = room.Id,
                Name = room.Name,
                Level = room.Level,
                InMantainance = room.InMantainance,
                Value = room.Price.Value,
                Currency = room.Price.Currency,
                Bookings = bookingsDTOs, 
            };
        }

        public static RoomDTO MapToAggregateDTO(Entities.Room room)
        {
            var bookingsDTOs = new List<BookingDTO>();
            foreach (var booking in room.Bookings)
            {
                bookingsDTOs.Add(BookingDTO.MapToDTO(booking));
            }

            return new RoomDTO
            {
                Id = room.Id,
                Name = room.Name,
                Level = room.Level,
                InMantainance = room.InMantainance,
                Value = room.Price.Value,
                Currency = room.Price.Currency,
                Bookings = bookingsDTOs,
            };
        }
    }
}
