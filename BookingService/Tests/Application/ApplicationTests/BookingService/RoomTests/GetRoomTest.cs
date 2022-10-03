using Application;
using Application.Room.Ports;
using Domain.Entities;
using Domain.Ports;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.BookingService.RoomTests
{
    public class GetRoomTest
    {
        private IRoomManager roomManager;

        [SetUp]
        public void Setup()
        {
            var fakeRepository = new RoomFakeRepository();
            roomManager = new RoomManager(fakeRepository);
        }

        [Test]
        public async Task ShouldRetriveRoomIfExists()
        {
            var id = 123;
            var response = await roomManager.GetRoom(id);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Sucess);
            Assert.AreEqual(id, response.Data.Id);
            Assert.AreEqual("Room's name", response.Data.Name);
            Assert.AreEqual(new List<Booking>(), response.Data.Bookings);
        }

        [Test]
        public async Task ShouldReturnNotFoundIfRoomDoesNotExist()
        {
            var id = 123;

            var fakeRepository = new Mock<IRoomRepository>();
            fakeRepository.Setup(x => x.Get(It.IsAny<int>()))
                .Returns(Task.FromResult<Room?>(null));
            roomManager = new RoomManager(fakeRepository.Object);

            var response = await roomManager.GetRoom(id);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.ROOM_NOT_FOUND);
            Assert.AreEqual(response.Message, "This room does not exist");
        }


        [Test]
        public async Task ShouldReturnUnknownErrorIfThrowsGenericException()
        {
            var id = 123;

            var fakeRepository = new Mock<IRoomRepository>();
            fakeRepository.Setup(x => x.Get(It.IsAny<int>()))
                .Throws(new Exception());
            roomManager = new RoomManager(fakeRepository.Object);

            var response = await roomManager.GetRoom(id);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.UNKNOWN_ERROR);
            Assert.AreEqual(response.Message, "Something went wrong while trying to comunicate with database");
        }
    }
}
