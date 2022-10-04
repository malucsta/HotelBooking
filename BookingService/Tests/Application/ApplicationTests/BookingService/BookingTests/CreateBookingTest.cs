using Application;
using Application.Booking.DTOs;
using Application.Booking.Ports;
using Application.Booking.Requests;
using ApplicationTests.BookingService.GuestTests;
using ApplicationTests.BookingService.RoomTests;
using Domain.Entities;
using Domain.Ports;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ApplicationTests.BookingService.BookingTests
{
    public class CreateBookingTest
    {
        private IBookingManager _bookingManager;

        private BookingDTO bookingDTO = new BookingDTO
        {
            Id = 0,
            PlacedAt = DateTime.Parse("2022-10-04T13:38:22.445Z"),
            Start = DateTime.Parse("2022-10-10T13:38:22.445Z"),
            End = DateTime.Parse("2022-10-12T13:38:22.445Z"),
            RoomId = 3,
            GuestId = 1,
            Status = 0,
        };

        [SetUp]
        public void Setup()
        {
            var guestFakeRepository = new GuestFakeRepository();
            var roomFakeRepository = new RoomFakeRepository();

            var bookingFakeRepository = new BookingFakeRepository();
            _bookingManager =
                new BookingManager(bookingFakeRepository, guestFakeRepository, roomFakeRepository);
        }

        [Test]
        public async Task ShouldCreateValidBooking()
        {

            var request = new CreateBookingRequest { Data = this.bookingDTO };
            var response = await _bookingManager.CreateBooking(request);

            Assert.IsNotNull(response);
            Assert.AreEqual(123, response.Data.Id);
            Assert.IsTrue(response.Sucess);
        }

        [Test]
        public async Task ShouldReturnInvalidFieldIfAlreadyHasBookingsForPeriod()
        {
            var guestFakeRepository = new GuestFakeRepository();
            var roomFakeRepository = new RoomFakeRepository();
            var bookingFakeRepository = new Mock<IBookingRepository>();
            bookingFakeRepository.Setup(x => x.CheckBookingsForPeriod(
                It.IsAny<int>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()
                )
            )
                .Returns(Task.FromResult<bool>(true));

            _bookingManager = new BookingManager(bookingFakeRepository.Object, guestFakeRepository, roomFakeRepository);

            var request = new CreateBookingRequest { Data = this.bookingDTO };
            var response = await _bookingManager.CreateBooking(request);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.BOOKING_INVALID_FIELD);
            Assert.AreEqual(response.Message, "This room already has bookings for this period");
        }

        [Test]
        public async Task ShouldReturnInvalidFieldIfStartOrEndDateIsPast()
        {
            this.bookingDTO.PlacedAt = DateTime.UtcNow;
            this.bookingDTO.Start = DateTime.UtcNow.AddDays(-2);

            var request = new CreateBookingRequest { Data = bookingDTO };
            var response = await _bookingManager.CreateBooking(request);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.BOOKING_INVALID_FIELD);
            Assert.AreEqual(response.Message, "Date cannot be past");
        }

        [Test]
        public async Task ShouldReturnInvalidFieldIfPeriodIsInvalid()
        {
            this.bookingDTO.PlacedAt = DateTime.UtcNow;
            this.bookingDTO.Start = DateTime.UtcNow.AddDays(3);
            this.bookingDTO.End = DateTime.UtcNow.AddDays(1);

            var request = new CreateBookingRequest { Data = bookingDTO };
            var response = await _bookingManager.CreateBooking(request);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.BOOKING_INVALID_FIELD);
            Assert.AreEqual(response.Message, "End date should come after start date");
        }

        [Test]
        public async Task ShouldReturnInvalidFieldIfRoomCannotBeBooked()
        {
            var bookingFakeRepository = new BookingFakeRepository();
            var guestFakeRepository = new GuestFakeRepository();
            var roomFakeRepository = new Mock<IRoomRepository>();
            roomFakeRepository.Setup(x => x.Get(
                It.IsAny<int>()))
                .Returns(Task.FromResult<Room?>(new Room
                {
                    Id = 1,
                    Name = "Room's name",
                    Level = 1,
                    InMantainance = true,
                    Price = new Domain.ValueObjects.Price
                    {
                        Value = 299.99M,
                        Currency = Domain.Enums.AcceptedCurrencies.BRL,
                    }
                }));

            _bookingManager = new BookingManager(bookingFakeRepository, guestFakeRepository, roomFakeRepository.Object);

            var request = new CreateBookingRequest { Data = bookingDTO };
            var response = await _bookingManager.CreateBooking(request);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.BOOKING_INVALID_FIELD);
            Assert.AreEqual(response.Message, "This room cannot be booked");
        }

        [Test]
        public async Task ShouldReturnNotFoundIfGuestDoesNotExist()
        {
            var bookingFakeRepository = new BookingFakeRepository();
            var roomFakeRepository = new RoomFakeRepository();
            var guestFakeRepository = new Mock<IGuestRepository>();
            guestFakeRepository.Setup(x => x.Get(
                It.IsAny<int>()))
                .Returns(Task.FromResult<Guest?>(null));

            _bookingManager = new BookingManager(bookingFakeRepository, guestFakeRepository.Object, roomFakeRepository);

            var request = new CreateBookingRequest { Data = bookingDTO };
            var response = await _bookingManager.CreateBooking(request);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.NOT_FOUND);
            Assert.AreEqual(response.Message, "This guest does not exist");
        }

        [Test]
        public async Task ShouldReturnNotFoundIfRoomDoesNotExist()
        {
            var bookingFakeRepository = new BookingFakeRepository();
            var guestFakeRepository = new GuestFakeRepository();
            var roomFakeRepository = new Mock<IRoomRepository>();
            roomFakeRepository.Setup(x => x.Get(
                It.IsAny<int>()))
                .Returns(Task.FromResult<Room?>(null));

            _bookingManager = new BookingManager(bookingFakeRepository, guestFakeRepository, roomFakeRepository.Object);

            var request = new CreateBookingRequest { Data = bookingDTO };
            var response = await _bookingManager.CreateBooking(request);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.ROOM_NOT_FOUND);
            Assert.AreEqual(response.Message, "This room does not exist");
        }

        [Test]
        public async Task ShouldReturnUnknownErrorIfGenericErrorOccurs()
        {
            var bookingFakeRepository = new BookingFakeRepository();
            var guestFakeRepository = new GuestFakeRepository();
            var roomFakeRepository = new Mock<IRoomRepository>();
            roomFakeRepository.Setup(x => x.Get(
                It.IsAny<int>()))
                .Throws(new Exception());

            _bookingManager = new BookingManager(bookingFakeRepository, guestFakeRepository, roomFakeRepository.Object);

            var request = new CreateBookingRequest { Data = bookingDTO };
            var response = await _bookingManager.CreateBooking(request);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.UNKNOWN_ERROR);
            Assert.AreEqual(response.Message, "Unknown error occurred");
        }
    }
}
