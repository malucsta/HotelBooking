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
    public class GetBookingTest
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
        public async Task ShouldReturnValidBooking()
        {
            var id = 123;
            var response = await _bookingManager.GetBooking(id); 
            
            Assert.IsNotNull(response);
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

            var id = 123;
            var response = await _bookingManager.GetBooking(id);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.BOOKING_NOT_FOUND);
        }
    }
}
