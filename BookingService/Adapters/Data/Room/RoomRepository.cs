using Domain.Ports;
using Microsoft.EntityFrameworkCore;
using Entities = Domain.Entities;

namespace Data.Room
{
    public class RoomRepository : IRoomRepository
    {
        private readonly HotelDbContext _context;
        public RoomRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Entities.Room room)
        {
            _context.Add(room);
            await _context.SaveChangesAsync();
            return room.Id;
        }

        public async Task<Entities.Room?> Get(int id)
        {
            var room = await _context.Rooms
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if(room != null)
                room.Bookings = new List<Entities.Booking>();
            
            return room;
        }
    }
}
