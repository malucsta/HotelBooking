using NUnit.Framework;
using Domain.Entities;
using Domain.Enums;

namespace DomainTests.BookingService
{
    public class StateMachineTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldAlwaysStartWithCreatedStatus()
        {
            var booking = new Booking();
            
            Assert.AreEqual(booking.Status, Status.Created);
        }

        [Test]
        public void ShouldSetStatusToPaidWhenPayingForBookingWithCreatedStatus()
        {
            var booking = new Booking();
            
            booking.ChangeState(Action.Pay);
            
            Assert.AreEqual(booking.Status, Status.Paid);
        }

        [Test]
        public void ShouldSetStatusToCanceledWhenCancelingCreatedBooking()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Cancel);

            Assert.AreEqual(booking.Status, Status.Canceled);
        }

        [Test]
        public void ShouldSetStatusToFinishedWhenFinishingPaidBooking()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Pay);
            booking.ChangeState(Action.Finish);

            Assert.AreEqual(booking.Status, Status.Finished);
        }

        [Test]
        public void ShouldSetStatusToRefoundedWhenRefoundingPaidBooking()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Pay);
            booking.ChangeState(Action.Refound);

            Assert.AreEqual(booking.Status, Status.Refounded);
        }

        [Test]
        public void ShouldSetStatusToCreatedWhenReopeningCanceledBooking()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Cancel);
            booking.ChangeState(Action.Reopen);

            Assert.AreEqual(booking.Status, Status.Created);
        }

        [Test]
        public void ShouldNotChangeStatusWhenRefoundingCreatedBooking()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Refound);

            Assert.AreEqual(booking.Status, Status.Created);
        }

        [Test]
        public void ShouldNotChangeStatusWhenRefoundingFinishedBooking()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Pay);
            booking.ChangeState(Action.Finish);
            booking.ChangeState(Action.Refound);

            Assert.AreEqual(booking.Status, Status.Finished);
        }

        [Test]
        public void ShouldNotChangeStatusWhenMantaining()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Pay);
            booking.ChangeState(Action.Mantain);

            Assert.AreEqual(booking.Status, Status.Paid);
        }
    }
}