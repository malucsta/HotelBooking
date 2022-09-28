using Domain.Entities;
using Domain.Ports;
using System.Threading.Tasks;

namespace ApplicationTests.BookingService.RoomTests
{
    public class RoomFakeRepository : IRoomRepository
    {
        public Task<int> Create(Room room)
        {
            return Task.FromResult(123);
        }

        public Task<Room?> Get(int id)
        {
            return Task.FromResult<Room?>(new Room {
                Id = id,
                Name = "Room's name",
                Level = 1,
                InMantainance = false, 
                Price = new Domain.ValueObjects.Price
                {
                    Value = 259.99M,
                    Currency = Domain.Enums.AcceptedCurrencies.BRL,
                },
            });
        }
    }
}
