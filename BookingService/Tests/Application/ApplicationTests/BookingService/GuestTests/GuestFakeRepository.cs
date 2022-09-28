using Domain.Enums;
using Domain.Ports;
using Guest = Domain.Entities.Guest;
using System.Threading.Tasks;

namespace ApplicationTests.BookingService.GuestTests
{
    public class GuestFakeRepository : IGuestRepository
    {
        public Task<int> Create(Guest guest)
        {
            return Task.FromResult(123);
        }

        public Task<Guest> Get(int id)
        {
            var guest = new Guest
            {
                Id = id,
                Name = "Guest",
                Surname = "Surname",
                Email = "guest@email.com",
                DocumentId = new Domain.ValueObjects.PersonId
                {
                    IdNumber = "123456",
                    DocumentType = (DocumentType)1,
                }
            };
            return Task.FromResult(guest);
        }
    }
}
