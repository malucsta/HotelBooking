using Application;
using Application.Booking.DTOs;
using Application.Room.DTOs;
using ApplicationTests.BookingService.BookingTests;
using Domain.Entities;
using Domain.Ports;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationTests.BookingService.RoomTests
{
    public class GetRoomAggregateTest
    {
        private RoomManager _roomManager;

        [SetUp]
        public void Setup()
        {
            var fakeRepository = new RoomFakeRepository();
            var fakeBookingRepository = new BookingFakeRepository();
            _roomManager = new RoomManager(fakeRepository, fakeBookingRepository);
        }

        [Test]
        public async Task ShouldReturnRoomAggregate()
        {
            var id = 1;
            var response = await _roomManager.GetRoomAggregate(id);

            var fakeBooking = new BookingDTO
            {
                Id = 1,
                PlacedAt = DateTime.UtcNow.ToLocalTime(),
                Start = DateTime.UtcNow.AddDays(1).ToLocalTime(),
                End = DateTime.UtcNow.AddDays(2).ToLocalTime(),
                RoomId = id,
                GuestId = 1,
            };

            var bookingsList = new List<BookingDTO>();
            bookingsList.Add(fakeBooking);

            var expectedResponse = new RoomDTO
            {
                Id = id,
                Name = "Room's name",
                Level = 1,
                InMantainance = false,
                Value = 259.99M,
                Currency = Domain.Enums.AcceptedCurrencies.BRL,
                Bookings = bookingsList
            };

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data.Bookings[0]);
            Assert.IsTrue(response.Sucess);
            Assert.AreEqual(expectedResponse.Name, response.Data.Name);
            Assert.AreEqual(response.Data.Bookings[0].GuestId, expectedResponse.Bookings[0].GuestId);
            Assert.AreEqual(response.Data.Bookings[0].RoomId, expectedResponse.Bookings[0].RoomId);
        }

        [Test]
        public async Task ShouldReturnRoomNotFoundIfRoomDoesNotExist()
        {
            var id = 1;

            var fakeRepository = new Mock<IRoomRepository>();
            var fakeBookingRepository = new BookingFakeRepository();
            fakeRepository.Setup(x => x.Get(It.IsAny<int>()))
                .Returns(Task.FromResult<Room?>(null));
            _roomManager = new RoomManager(fakeRepository.Object, fakeBookingRepository);

            var response = await _roomManager.GetRoomAggregate(id);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.ROOM_NOT_FOUND);
            Assert.AreEqual(response.Message, "This room does not exist");
        }

        [Test]
        public async Task ShouldReturnUnknownErrorIfGenericErrorOccurs()
        {
            var id = 1;

            var fakeRepository = new Mock<IRoomRepository>();
            var fakeBookingRepository = new BookingFakeRepository();
            fakeRepository.Setup(x => x.Get(It.IsAny<int>()))
                .Throws(new Exception());
            _roomManager = new RoomManager(fakeRepository.Object, fakeBookingRepository);

            var response = await _roomManager.GetRoomAggregate(id);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.UNKNOWN_ERROR);
            Assert.AreEqual(response.Message, "Something went wrong while trying to comunicate with database");
        }
    }
}
