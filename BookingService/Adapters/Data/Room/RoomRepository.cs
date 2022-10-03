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

        public async Task<Entities.Room?> GetAggregate(int id)
        {
            //retrives room data
            var room = await this.Get(id);

            if(room == null)
                return null;

            //retrives bookings for this specific room
            var result = await _context.Bookings
                .Include(x => x.Guest)
                .Select(booking => new RoomAggregateDTO
                {
                    Id = booking.Id,
                    PlacedAt = booking.PlacedAt,
                    Start = booking.Start,
                    End = booking.End,
                    RoomId = booking.Room.Id,
                    GuestId = booking.Guest.Id,
                })
                .Where(booking => booking.RoomId == id)
                .ToListAsync();

            var bookingList = new List<Entities.Booking>();

            foreach(RoomAggregateDTO booking in result)
            {
                bookingList.Add(RoomAggregateDTO.MapToEntity(booking));
            }

            room.Bookings = bookingList;
            
            return room;
        }
    }
}
