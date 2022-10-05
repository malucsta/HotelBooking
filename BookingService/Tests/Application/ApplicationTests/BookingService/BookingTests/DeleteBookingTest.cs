using Application;
using Application.Booking.DTOs;
using Application.Booking.Ports;
using ApplicationTests.BookingService.GuestTests;
using ApplicationTests.BookingService.RoomTests;
using Domain.Entities;
using Domain.Ports;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.BookingService.BookingTests
{
    public class DeleteBookingTest
    {
        private IBookingManager _bookingManager;

        private BookingDTO bookingDTO = new BookingDTO
        {
            Id = 123,
            PlacedAt = DateTime.Parse("2022-10-04T13:38:22.445Z").ToLocalTime(),
            Start = DateTime.Parse("2022-10-10T13:38:22.445Z").ToLocalTime(),
            End = DateTime.Parse("2022-10-12T13:38:22.445Z").ToLocalTime(),
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
        public async Task ShouldDeleteBooking()
        {
            int id = 123;
            var response = await _bookingManager.DeleteBooking(id);

            Assert.IsTrue(response.Sucess);
            Assert.AreEqual(response.Data.Id, bookingDTO.Id); 
            Assert.AreEqual(response.Data.RoomId, bookingDTO.RoomId);
            Assert.AreEqual(response.Data.GuestId, bookingDTO.GuestId);
        }

        [Test]
        public async Task ShouldReturnBookingNotFoundIfBookingDoesNotExists()
        {
            var guestFakeRepository = new GuestFakeRepository();
            var roomFakeRepository = new RoomFakeRepository();
            var bookingFakeRepository = new Mock<IBookingRepository>();
            bookingFakeRepository.Setup(x => x.GetBooking(It.IsAny<int>()))
                .Returns(Task.FromResult<Booking?>(null));

            _bookingManager = new BookingManager(bookingFakeRepository.Object, guestFakeRepository, roomFakeRepository);

            int id = 123;
            var response = await _bookingManager.DeleteBooking(id);

            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.BOOKING_NOT_FOUND); 
        }

        [Test]
        public async Task ShouldReturnInvalidOperationIfStatusIsOtherThanCreated()
        {
            var booking = new Booking
            {
                Id = 123,
                PlacedAt = DateTime.Parse("2022-10-04T13:38:22.445Z"),
                Start = DateTime.Parse("2022-10-10T13:38:22.445Z"),
                End = DateTime.Parse("2022-10-12T13:38:22.445Z"),
                Room = new Room
                {
                    Id = 3,
                },
                Guest = new Guest
                {
                    Id = 1,
                }
            };

            booking.ChangeState(Domain.Enums.Action.Pay); 

            var guestFakeRepository = new GuestFakeRepository();
            var roomFakeRepository = new RoomFakeRepository();
            var bookingFakeRepository = new Mock<IBookingRepository>();
            bookingFakeRepository.Setup(x => x.GetBooking(It.IsAny<int>()))
                .Returns(Task.FromResult<Booking?>(booking));

            _bookingManager = new BookingManager(bookingFakeRepository.Object, guestFakeRepository, roomFakeRepository);

            int id = 123;
            var response = await _bookingManager.DeleteBooking(id);

            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.BOOKING_INVALID_OPERATION);
        }
    }
}
