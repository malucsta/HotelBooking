﻿using Application.Booking.DTOs;
using Application.Booking.Exceptions;
using Application.Booking.Ports;
using Application.Booking.Requests;
using Application.Booking.Responses;
using Domain.Booking.Exceptions;
using Domain.Ports;

namespace Application
{
    public class BookingManager : IBookingManager
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IRoomRepository _roomRepository;

        public BookingManager(
            IBookingRepository bookingRepository, 
            IGuestRepository guestRepository, 
            IRoomRepository roomRepository
            )
        {
            _bookingRepository = bookingRepository;
            _guestRepository = guestRepository;
            _roomRepository = roomRepository;
        }
        public async Task<BookingResponse> CreateBooking(CreateBookingRequest request)
        {
            try
            {
                var booking = BookingDTO.MapToEntity(request.Data);
                var guest = await _guestRepository.Get(request.Data.GuestId);
                var room = await _roomRepository.Get(request.Data.RoomId);

                if (guest == null) throw new GuestNotFoundException();
                if (room == null) throw new RoomNotFoundException(); 
                
                booking.Guest = guest; 
                booking.Room = room; 

                await booking.Save(_bookingRepository);
                request.Data.Id = booking.Id;

                return new BookingResponse
                {
                    Data = request.Data,
                    Sucess = true,
                };
            }
            catch(InvalidDateException e)
            {
                return new BookingResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.BOOKING_INVALID_FIELD,
                    Message = e.Message,
                };
            }

            catch (InvalidPeriodException e)
            {
                return new BookingResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.BOOKING_INVALID_FIELD,
                    Message = e.Message,
                };
            }

            catch (InvalidGuestException e)
            {
                return new BookingResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.BOOKING_INVALID_FIELD,
                    Message = e.Message,
                };
            }

            catch (RoomCannotBeBookedException)
            {
                return new BookingResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.BOOKING_INVALID_FIELD,
                    Message = "This room cannot be booked",
                };
            }

            catch (GuestNotFoundException)
            {
                return new BookingResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.NOT_FOUND,
                    Message = "This guest does not exist",
                };
            }

            catch (RoomNotFoundException)
            {
                return new BookingResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.ROOM_NOT_FOUND,
                    Message = "This room does not exist",
                };
            }

            catch (Exception)
            {
                return new BookingResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.UNKNOWN_ERROR,
                    Message = "Unknown error occurred",
                };
            }
        }

        public async Task<BookingResponse> GetBooking(int bookingID)
        {
            throw new NotImplementedException();
        }
    }
}