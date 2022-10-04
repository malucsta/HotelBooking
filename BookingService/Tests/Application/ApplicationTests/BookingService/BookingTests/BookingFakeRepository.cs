using Domain.Entities;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationTests.BookingService.BookingTests
{
    public class BookingFakeRepository : IBookingRepository
    {
        public Task<bool> CheckBookingsForPeriod(int roomID, DateTime start, DateTime end)
        {
            return Task.FromResult(false);
        }

        public Task<int> CreateBooking(Booking booking)
        {
            return Task.FromResult<int>(123);
        }

        public Task<Booking?> GetBooking(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Booking>> GetBookingsByRoom(int roomID)
        {
            throw new NotImplementedException();
        }
    }
}
