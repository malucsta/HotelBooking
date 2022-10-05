using Domain.Entities;

namespace Domain.Ports
{
    public interface IRoomRepository
    {
        Task<Room?> GetRoom (int id);
        Task<int> CreateRoom (Room room);
        Task<Room> UpdateRoom (Room room);
        Task<Room> ToggleMantainanceStatus(Room room);

    }
}
