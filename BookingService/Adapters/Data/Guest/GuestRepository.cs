using Domain.Ports;
using Microsoft.EntityFrameworkCore;
using Entities = Domain.Entities;

namespace Data.Guest
{
    public class GuestRepository : IGuestRepository
    {
        private readonly HotelDbContext _context;
        public GuestRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckBookingsForGuest(int id)
        {
            var bookings = await _context.Bookings
                .Include(x => x.Guest)
                .Where(x => x.Guest.Id == id)
                .ToListAsync();

            return bookings.Count > 0;
        }

        public async Task<int> Create(Entities.Guest guest)
        {
            _context.Add(guest);
            await _context.SaveChangesAsync();
            return guest.Id;
        }

        public async Task<int> Delete(Entities.Guest guest)
        {
            _context.Remove(guest);
            await _context.SaveChangesAsync();
            return guest.Id; 
        }

        public Task<Entities.Guest?> Get(int id)
        {
            return _context.Guests.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
