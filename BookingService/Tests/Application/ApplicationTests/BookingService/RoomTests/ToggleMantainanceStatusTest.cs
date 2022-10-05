using Application;
using Application.Booking.DTOs;
using Application.Room.DTOs;
using Application.Room.Ports;
using ApplicationTests.BookingService.BookingTests;
using Domain.Entities;
using Domain.Ports;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationTests.BookingService.RoomTests
{
    public class ToggleMantainanceStatusTest
    {
        private IRoomManager roomManager;

        [SetUp]
        public void Setup()
        {
            var fakeRoomRepository = new RoomFakeRepository();
            var fakeBookingRepository = new BookingFakeRepository();
            roomManager = new RoomManager(fakeRoomRepository, fakeBookingRepository);
        }

        [Test]
        public async Task ShouldUpdateMantainanceStatusIfRoomExists()
        {
            var id = 123;
            var response = await roomManager.ToggleMantainanceStatus(id);

            var roomDTO = new RoomDTO
            {
                Id = id,
                Name = "Room's name",
                Level = 1,
                InMantainance = true,
                Value = 259.99M,
                Currency = Domain.Enums.AcceptedCurrencies.BRL,
                Bookings = new List<BookingDTO>(),
            };

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Sucess);
            Assert.AreEqual("Room's name", response.Data.Name);
            Assert.AreNotSame(response.Data.InMantainance, roomDTO.InMantainance);
        }

        [Test]
        public async Task ShouldReturnNotFoundIfRoomDoesNotExist()
        {
            var id = 123;

            var fakeRepository = new Mock<IRoomRepository>();
            var fakeBookingRepository = new BookingFakeRepository();
            fakeRepository.Setup(x => x.GetRoom(It.IsAny<int>()))
                .Returns(Task.FromResult<Room?>(null));
            roomManager = new RoomManager(fakeRepository.Object, fakeBookingRepository);

            var response = await roomManager.ToggleMantainanceStatus(id);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.ROOM_NOT_FOUND);
            Assert.AreEqual(response.Message, "This room does not exist");
        }
    }
}
