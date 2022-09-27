using Domain.Ports;
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
        public async Task<int> Create(Entities.Guest guest)
        {
            _context.Add(guest);
            await _context.SaveChangesAsync();
            return guest.Id;
        }

        public Task<Entities.Guest> Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
