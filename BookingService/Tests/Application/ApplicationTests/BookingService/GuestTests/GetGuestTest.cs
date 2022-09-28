using Application;
using Domain.Entities;
using Domain.Ports;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace ApplicationTests.BookingService.GuestTests
{
    public class GetGuestTest
    {
        private GuestManager guestManager;

        [SetUp]
        public void Setup()
        {
            var fakeRepository = new GuestFakeRepository();
            guestManager = new GuestManager(fakeRepository);
        }

        [Test]
        public async Task ShouldRetriveGuestIfExists()
        {
            var id = 123;
            var response = await guestManager.GetGuest(id);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Sucess);
            Assert.AreEqual(id, response.Data.Id);
            Assert.AreEqual("Guest", response.Data.Name);
        }

        [Test]
        public async Task ShouldReturnNotFoundIfGuestDoesNotExist()
        {
            var id = 123;

            var fakeRepository = new Mock<IGuestRepository>();
            fakeRepository.Setup(x => x.Get(It.IsAny<int>()))
                .Returns(Task.FromResult<Guest?>(null));
            guestManager = new GuestManager(fakeRepository.Object);

            var response = await guestManager.GetGuest(id);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.NOT_FOUND);
            Assert.AreEqual(response.Message, "This guest doesn't exist");
        }
    }
}
