﻿using Domain.Ports;
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

        public Task<Entities.Room?> Get(int id)
        {
            return _context.Rooms
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(); 
        }

        public Task<Entities.Room?> GetAggregate(int id)
        {
            return _context.Rooms
                .Include(r => r.Bookings)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
