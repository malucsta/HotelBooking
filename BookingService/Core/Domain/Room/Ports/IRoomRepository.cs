using Domain.Entities;

namespace Domain.Ports
{
    public interface IRoomRepository
    {
        Task<Room?> Get(int id);
        Task<Room?> GetAggregate(int id);
        Task<int> Create(Room room);

    }
}
