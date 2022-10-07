using Application;
using Application.Guest.Ports;
using Domain.Entities;
using Domain.Enums;
using Domain.Ports;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace ApplicationTests.BookingService.GuestTests
{
    public class DeleteGuestTest
    {
        private IGuestManager guestManager;
        
        [SetUp]
        public void Setup()
        {
            var fakeRepository = new GuestFakeRepository();
            guestManager = new GuestManager(fakeRepository);
        }

        [Test]
        public async Task ShouldDeleteGuestIfExists()
        {
            var id = 123;
            var response = await guestManager.DeleteGuest(id);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Sucess);
            Assert.AreEqual(id, response.Data.Id);
            Assert.AreEqual("Guest", response.Data.Name);
        }

        [Test]
        public async Task ShouldReturnNotFoundIfGuestDoesNotExists()
        {
            var fakeRepository = new Mock<IGuestRepository>();
            fakeRepository.Setup(x => x.Get(It.IsAny<int>()))
                .Returns(Task.FromResult<Guest?>(null));
            guestManager = new GuestManager(fakeRepository.Object);

            var id = 123;
            var response = await guestManager.DeleteGuest(id);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.NOT_FOUND);
        }

        [Test]
        public async Task ShouldReturnInvalidOperationIfGuestHasBookings()
        {
            var id = 123;
            var guest = new Guest
            {
                Id = id,
                Name = "Guest",
                Surname = "Surname",
                Email = "guest@email.com",
                DocumentId = new Domain.ValueObjects.PersonId
                {
                    IdNumber = "123456",
                    DocumentType = (DocumentType)1,
                }
            };

            var fakeRepository = new Mock<IGuestRepository>();
            fakeRepository.Setup(x => x.CheckBookingsForGuest(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            fakeRepository.Setup(x => x.Get(It.IsAny<int>()))
                .Returns(Task.FromResult<Guest?>(guest));

            guestManager = new GuestManager(fakeRepository.Object);

            
            var response = await guestManager.DeleteGuest(id);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(response.ErrorCode, ErrorCode.INVALID_OPERATION);
        }
    }
}
