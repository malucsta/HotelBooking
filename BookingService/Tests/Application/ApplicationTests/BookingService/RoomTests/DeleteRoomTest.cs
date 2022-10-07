using Application;
using Application.Room.Ports;
using ApplicationTests.BookingService.BookingTests;
using Domain.Entities;
using Domain.Ports;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace ApplicationTests.BookingService.RoomTests
{
    public class DeleteRoomTest
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
        public async Task ShouldDeleteRoomIfExists()
        {
            var id = 123;
            var response = await roomManager.DeleteRoom(id);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Sucess);
            Assert.AreEqual(id, response.Data.Id);
            Assert.AreEqual("Room's name", response.Data.Name);
        }

        [Test]
        public async Task ShouldReturnNotFoundIfRoomDoesNotExists()
        {
            var fakeRepository = new Mock<IRoomRepository>();
            var fakeBookingRepository = new BookingFakeRepository();
            fakeRepository.Setup(x => x.GetRoom(It.IsAny<int>()))
                .Returns(Task.FromResult<Room?>(null));
            roomManager = new RoomManager(fakeRepository.Object, fakeBookingRepository);

            var id = 123;
            var response = await roomManager.DeleteRoom(id);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.ROOM_NOT_FOUND);
        }

        [Test]
        public async Task ShouldReturnInvalidOperationIfRoomHasBookings()
        {
            var fakeRepository = new RoomFakeRepository();
            var fakeBookingRepository = new Mock<IBookingRepository>();
            fakeBookingRepository.Setup(x => x.CheckBookingsForRoom(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            roomManager = new RoomManager(fakeRepository, fakeBookingRepository.Object);

            var id = 123;
            var response = await roomManager.DeleteRoom(id);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.ROOM_INVALID_OPERATION);
        }
    }
}
