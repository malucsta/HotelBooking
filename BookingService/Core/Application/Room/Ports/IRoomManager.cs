using Application.Room.Requests;
using Application.Room.Responses;

namespace Application.Room.Ports
{
    public interface IRoomManager
    {
        Task<RoomResponse> CreateRoom(CreateRoomRequest request);

        Task<RoomResponse> GetRoom(int roomID);
        Task<RoomResponse> GetRoomAggregate(int roomID);
        Task<RoomResponse> ToggleMantainanceStatus(int roomID);
    }
}
