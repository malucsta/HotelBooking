using Application.Room.DTOs;
using Application.Room.Ports;
using Application.Room.Requests;
using Application.Room.Responses;
using Domain.DomainExceptions;
using Domain.Ports;
using System.Text.Json;

namespace Application
{
    public class RoomManager : IRoomManager
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IBookingRepository _bookingRepository;

        public RoomManager(IRoomRepository roomRepository, IBookingRepository bookingRepository)
        {
            _roomRepository = roomRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<RoomResponse> CreateRoom(CreateRoomRequest request)
        {
            try
            {
                var room = RoomDTO.MapToEntity(request.Data);
                await room.Save(_roomRepository);
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
                var room = await _roomRepository.Get(roomID);

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

        public async Task<RoomResponse> GetRoomAggregate(int roomID)
        {
            try
            {
                var room = await _roomRepository.Get(roomID);

                if(room is null)
                {
                    return new RoomResponse
                    {
                        Sucess = false,
                        ErrorCode = ErrorCode.ROOM_NOT_FOUND,
                        Message = "This room does not exist"
                    };
                }
                
                var bookingsByRoom = await _bookingRepository.GetBookingsByRoom(roomID);

                if(bookingsByRoom is null)
                {
                    return new RoomResponse
                    {
                        Sucess = false,
                        ErrorCode = ErrorCode.BOOKING_NOT_FOUND,
                        Message = "This room does not have bookings"
                    };
                }

                room.Bookings = bookingsByRoom;
                var roomDTO = RoomDTO.MapToAggregateDTO(room);

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
