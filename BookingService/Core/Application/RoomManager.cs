using Application.Room.DTOs;
using Application.Room.Ports;
using Application.Room.Requests;
using Application.Room.Responses;
using Domain.DomainExceptions;
using Domain.Ports;

namespace Application
{
    public class RoomManager : IRoomManager
    {
        private readonly IRoomRepository _repository;

        public RoomManager(IRoomRepository repository)
        {
            _repository = repository;
        }

        public async Task<RoomResponse> CreateRoom(CreateRoomRequest request)
        {
            try
            {
                var room = RoomDTO.MapToEntity(request.Data);
                await room.Save(_repository);
                request.Data.Id = room.Id;

                return new RoomResponse
                {
                    Data = request.Data,
                    Sucess = true,
                };
            }
            catch (MissingFieldsException e)
            {
                return new RoomResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.ROOM_MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required fields information"
                };
            }
            catch (InvalidFieldException e)
            {
                return new RoomResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.ROOM_INVALID_FIELD,
                    Message = $"Invalid field: {e.Message}"
                };
            }
            catch (Exception e)
            {
                return new RoomResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.ROOM_COULD_NOT_STORE_DATA,
                    Message = "Something went wrong while trying to save room to database"
                };
            }
        }

        public async Task<RoomResponse> GetRoom(int roomID)
        {
            try
            {
                var room = await _repository.Get(roomID);

                if (room == null)
                {
                    return new RoomResponse
                    {
                        Sucess = false,
                        ErrorCode = ErrorCode.ROOM_NOT_FOUND,
                        Message = "This room does not exist"
                    };
                }

                var roomDTO = RoomDTO.MapToDTO(room);

                return new RoomResponse
                {
                    Sucess = true,
                    Data = roomDTO,
                };
            }

            catch (Exception e)
            {
                return new RoomResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.UNKNOWN_ERROR,
                    Message = "Something went wrong while trying to comunicate with database"
                };
            }
        }
    }
}
