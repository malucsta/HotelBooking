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

        public async Task<List<Entities.Booking>?> GetBookingsByRoom(int roomID)
        {
            var result = await _context.Bookings
                .Include(x => x.Guest)
                .Select(booking => new BookingDTO
                {
                    Id = booking.Id,
                    PlacedAt = booking.PlacedAt,
                    Start = booking.Start,
                    End = booking.End,
                    RoomId = booking.Room.Id,
                    GuestId = booking.Guest.Id,
                })
                .Where(booking => booking.RoomId == roomID)
                .ToListAsync();

            if (result is null || result.Count == 0)
                return null;

            var bookingList = new List<Entities.Booking>();

            foreach (BookingDTO booking in result)
            {
                bookingList.Add(BookingDTO.MapToEntity(booking));
            }

            return bookingList;
        }

        public async Task<int> CreateBooking(Entities.Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking.Id;
        }

        public async Task<Entities.Booking?> GetBooking(int id)
        {
            var booking = await _context.Bookings
                .Include(x => x.Guest)
                .Select(booking => new BookingDTO
                {
                    Id = booking.Id,
                    PlacedAt = booking.PlacedAt,
                    Start = booking.Start,
                    End = booking.End,
                    RoomId = booking.Room.Id,
                    GuestId = booking.Guest.Id,
                })
                .Where(booking => booking.Id == id)
                .FirstOrDefaultAsync();

            if(booking is not null)
                return BookingDTO.MapToEntity(booking);

            return null; 
        }

        public async Task<bool> CheckBookingsForPeriod(int roomID, DateTime start, DateTime end)
        {
            var bookings = await _context.Bookings
                 .Where(x => x.Room.Id == roomID && 
                 x.Start.Date <= end.Date &&
                 x.End.Date >= start.Date)
                 .ToListAsync();

            return bookings.Count > 0; 
        }
    }
}
