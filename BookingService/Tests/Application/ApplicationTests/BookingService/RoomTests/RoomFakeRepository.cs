using Domain.Entities;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationTests.BookingService.RoomTests
{
    public class RoomFakeRepository : IRoomRepository
    {
        public Task<int> CreateRoom(Room room)
        {
            return Task.FromResult(123);
        }

        public Task<Room?> GetRoom(int id)
        {
            return Task.FromResult<Room?>(new Room
            {
                Id = id,
                Name = "Room's name",
                Level = 1,
                InMantainance = false,
                Price = new Domain.ValueObjects.Price
                {
                    Value = 259.99M,
                    Currency = Domain.Enums.AcceptedCurrencies.BRL,
                },
                Bookings = new List<Booking>(),
            }); ;
        }

        public Task<Room?> GetAggregate(int id)
        {
            var booking = new Booking
            {

                Id = 1,
                PlacedAt = DateTime.UtcNow,
                Start = DateTime.UtcNow.AddDays(1),
                End = DateTime.UtcNow.AddDays(2),
                Room = new Room
                {
                    Id = id,
                },
                Guest = new Guest
                {
                    Id = 1,
                }
            };

            var bookingList = new List<Booking>();
            bookingList.Add(booking); 

            var roomAggregate = new Room
            {
                Id = id,
                Name = "Room's name",
                Level = 1,
                InMantainance = false,
                Price = new Domain.ValueObjects.Price
                {
                    Value = 259.99M,
                    Currency = Domain.Enums.AcceptedCurrencies.BRL,
                },
                Bookings = bookingList,
            };

            return Task.FromResult<Room?>(roomAggregate);
        }

        public Task<Room> UpdateRoom(Room room)
        {
            throw new NotImplementedException();
        }

        public Task<Room> ToggleMantainanceStatus(Room room)
        {
            room.InMantainance = !room.InMantainance;
            return Task.FromResult(room);
        }

        public Task<int> DeleteRoom(Room room)
        {
            return Task.FromResult(room.Id);
        }
    }
}
