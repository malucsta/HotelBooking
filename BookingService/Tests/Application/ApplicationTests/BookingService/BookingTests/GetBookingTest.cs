using Application;
using Application.Booking.Ports;
using ApplicationTests.BookingService.GuestTests;
using ApplicationTests.BookingService.RoomTests;
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

        }
    }
}
