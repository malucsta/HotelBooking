using Application;
using Application.Room.DTOs;
using Application.Room.Ports;
using Application.Room.Requests;
using ApplicationTests.BookingService.BookingTests;
using Domain.Entities;
using Domain.Ports;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ApplicationTests.BookingService.RoomTests
{
    public class CreateRoomTest
    {
        private IRoomManager _roomManager;

        [SetUp]
        public void Setup()
        {
            var fakeRoomRepository = new RoomFakeRepository();
            var fakeBookingRepository = new BookingFakeRepository();
            _roomManager = new RoomManager(fakeRoomRepository, fakeBookingRepository);
        }

        [Test] 
        public async Task ShouldCreateValidRoom()
        {
            var roomDTO = new RoomDTO
            {
                Id = 0,
                Name = "Room's name",
                Level = 1,
                InMantainance = false,
                Value = 259.99M,
                Currency = Domain.Enums.AcceptedCurrencies.BRL,
            };

            var request = new CreateRoomRequest
            {
                Data = roomDTO,
            };

            var response = await _roomManager.CreateRoom(request);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Sucess);
            Assert.AreEqual(123, request.Data.Id);
        }

        [TestCase(null, null, null)]
        public async Task ShouldCreateValidRoomWithDefaultValues(int level, bool inMantainance, Domain.Enums.AcceptedCurrencies currency)
        {
            var roomDTO = new RoomDTO
            {
                Id = 0,
                Name = "Room's name",
                Level = level,
                InMantainance = inMantainance,
                Value = 259.99M,
                Currency = currency,
            };

            var request = new CreateRoomRequest
            {
                Data = roomDTO,
            };

            var response = await _roomManager.CreateRoom(request);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Sucess);
            Assert.AreEqual(123, request.Data.Id);
        }

        [TestCase("")]
        [TestCase(null)]
        public async Task ShouldThrowMissingFieldsExceptionIfRoomHasInvalidName(string name)
        {
            var roomDTO = new RoomDTO
            {
                Id = 0,
                Name = name,
                Level = 1,
                InMantainance = false,
                Value = 199.9M,
                Currency = Domain.Enums.AcceptedCurrencies.BRL,
            };

            var request = new CreateRoomRequest
            {
                Data = roomDTO,
            };

            var response = await _roomManager.CreateRoom(request);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.ROOM_MISSING_REQUIRED_INFORMATION);
            Assert.AreEqual(response.Message, "Missing required fields information");
        }

        [TestCase(-1)]
        public async Task ShouldThrowInvalidFieldExceptionIfRoomLevelIsnvalid(int level)
        {
            var roomDTO = new RoomDTO
            {
                Id = 0,
                Name = "Room's name",
                Level = level,
                InMantainance = false,
                Value = 199.9M,
                Currency = Domain.Enums.AcceptedCurrencies.BRL,
            };

            var request = new CreateRoomRequest
            {
                Data = roomDTO,
            };

            var response = await _roomManager.CreateRoom(request);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.ROOM_INVALID_FIELD);
            Assert.AreEqual(response.Message, "Invalid field: level");
        }

        [TestCase(-100)]
        [TestCase(0)]
        public async Task ShouldThrowInvalidFieldExceptionIfRoomPriceIsnvalid(decimal price)
        {
            var roomDTO = new RoomDTO
            {
                Id = 0,
                Name = "Room's name",
                Level = 1,
                InMantainance = false,
                Value = price,
                Currency = Domain.Enums.AcceptedCurrencies.BRL,
            };

            var request = new CreateRoomRequest
            {
                Data = roomDTO,
            };

            var response = await _roomManager.CreateRoom(request);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.ROOM_INVALID_FIELD);
            Assert.AreEqual(response.Message, "Invalid field: price");
        }

        [Test]
        public async Task ShouldCatchGenericExceptionWhenDataCannotBeStored()
        {
            var fakeRepository = new Mock<IRoomRepository>();
            var fakeBookingRepository = new BookingFakeRepository();
            fakeRepository.Setup(x => x.CreateRoom(It.IsAny<Room>()))
                .Throws(new Exception());
            _roomManager = new RoomManager(fakeRepository.Object, fakeBookingRepository);

            var roomDTO = new RoomDTO
            {
                Id = 0,
                Name = "Room's name",
                Level = 1,
                InMantainance = false,
                Value = 199.9M,
                Currency = Domain.Enums.AcceptedCurrencies.BRL,
            };

            var request = new CreateRoomRequest
            {
                Data = roomDTO,
            };

            var response = await _roomManager.CreateRoom(request);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.ROOM_COULD_NOT_STORE_DATA);
            Assert.AreEqual(response.Message, "Something went wrong while trying to save room to database");
        }
    }
}
