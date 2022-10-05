using Domain.Entities;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationTests.BookingService.BookingTests
{
    public class BookingFakeRepository : IBookingRepository
    {
        public Task<bool> CheckBookingsForRoomByPeriod(int roomID, DateTime start, DateTime end)
        {
            return Task.FromResult(false);
        }

        public Task<int> CreateBooking(Booking booking)
        {
            return Task.FromResult<int>(123);
        }

        public Task DeleteBooking(Booking booking)
        {
            return Task.CompletedTask; 
        }

        public Task<Booking?> GetBooking(int id)
        {
            var booking = new Booking
            {
                Id = 123,
                PlacedAt = DateTime.Parse("2022-10-04T13:38:22.445Z"),
                Start = DateTime.Parse("2022-10-10T13:38:22.445Z"),
                End = DateTime.Parse("2022-10-12T13:38:22.445Z"),
                Room = new Room
                {
                    Id = 3,
                },
                Guest = new Guest
                {
                    Id = 1,
                }
            };

            return Task.FromResult((Booking?) booking);
    }

        public Task<List<Booking>> GetBookingsByRoom(int roomID)
        {
            var booking = new Booking {
                Id = 1,
                PlacedAt = DateTime.UtcNow.ToLocalTime(),
                Start = DateTime.UtcNow.AddDays(1).ToLocalTime(),
                End = DateTime.UtcNow.AddDays(2).ToLocalTime(),
                Room = new Room
                {
                    Id = roomID,
                },
                Guest = new Guest
                {
                    Id = 1,
                },
            };

            var list = new List<Booking>(); 
            list.Add(booking);  
            return Task.FromResult(list); 
        }
    }
}
