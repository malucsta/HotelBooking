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

        public async Task<int> CreateRoom(Entities.Room room)
        {
            _context.Add(room);
            await _context.SaveChangesAsync();
            return room.Id;
        }

        public async Task<int> DeleteRoom(Entities.Room room)
        {
            _context.Remove(room);
            await _context.SaveChangesAsync();
            return room.Id;
        }

        public async Task<Entities.Room?> GetRoom(int id)
        {
            var room = await _context.Rooms
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if(room != null)
                room.Bookings = new List<Entities.Booking>();
            
            return room;
        }

        public async Task<Entities.Room> ToggleMantainanceStatus(Entities.Room room)
        {
            _context.Entry(room).Property(x => x.InMantainance).IsModified = true;
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<Entities.Room> UpdateRoom(Entities.Room room)
        {
            _context.Update(room);
            await _context.SaveChangesAsync();
            return room;
        }
    }
}
