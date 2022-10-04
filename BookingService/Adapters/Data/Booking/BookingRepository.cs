using Domain.Ports;
using Entities = Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.Booking
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelDbContext _context;

        public BookingRepository(HotelDbContext context)
        {
            _context = context; 
        }

        public async Task<List<Entities.Booking>> GetBookingsByRoom(int roomID)
        {
            return await _context.Bookings
                .Where(x => x.Room.Id == roomID)
                .ToListAsync();
        }

        public async Task<int> CreateBooking(Entities.Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking.Id;
        }

        public async Task<Entities.Booking?> GetBooking(int id)
        {
            return await _context.Bookings.Where(x => x.Id == id).FirstOrDefaultAsync(); 
        }

        public async Task<bool> CheckBookingsForPeriod(int roomID, DateTime start, DateTime end)
        {
            var bookings = await _context.Bookings
                 .Where(x => x.Room.Id == roomID && 
                 x.Start.Date <= end.Date && 
                 start.Date <= x.End.Date)
                 .ToListAsync();

            return bookings.Count > 0; 
        }
    }
}
