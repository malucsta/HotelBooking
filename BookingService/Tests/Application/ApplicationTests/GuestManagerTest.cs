using Application;
using Application.Guest.DTOs;
using Application.Guest.Requests;
using Domain.Entities;
using Domain.Ports;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ApplicationTests
{
    internal class FakeRepository : IGuestRepository
    {
        public Task<int> Create(Guest guest)
        {
            return Task.FromResult(123);
        }

        public Task<Guest> Get(int id)
        {
            throw new System.NotImplementedException();
        }
    }
    public class GuestManagerTest
    {
        GuestManager guestManager;

        [SetUp]
        public void Setup()
        {
            var fakeRepository = new FakeRepository();
            guestManager = new GuestManager(fakeRepository);
        }

        [Test]
        public async Task ShouldCreateValidGuest()
        {
            var guestDTO = new GuestDTO
            {
                Id = 0,
                Name = "Some",
                Surname = "Guest",
                Email = "guest@email.com",
                IdNumber = "123456",
                DocumentType = 1,
            };

            var request = new CreateGuestRequest { Data = guestDTO };

            var response = await guestManager.CreateGuest(request);

            Assert.IsNotNull(response);
            Assert.AreEqual(123, response.Data.Id);
            Assert.IsTrue(response.Sucess);
        }

        [TestCase("")]
        [TestCase("a")]
        [TestCase("ab")]
        [TestCase("abc")]
        [TestCase(null)]
        public async Task ShouldCatchInvalidDocumentExceptionWhenDocIdIsInvalid(string docNumber)
        {
            var guestDTO = new GuestDTO
            {
                Id = 0,
                Name = "Some",
                Surname = "Guest",
                Email = "guest@email.com",
                IdNumber = docNumber,
                DocumentType = 1,
            };

            var request = new CreateGuestRequest { Data = guestDTO };

            var response = await guestManager.CreateGuest(request);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(ErrorCode.INVALID_PERSON_ID, response.ErrorCode);
            Assert.AreEqual("Document Id is invalid", response.Message);
        }


        [TestCase("", "name", "guest@email.com")]
        [TestCase(null, "name", "guest@email.com")]
        
        [TestCase("some", "", "guest@email.com")]
        [TestCase("some", null, "guest@email.com")]
        
        [TestCase("some", "name", "")]
        [TestCase("some", "name", null)]
        
        public async Task ShouldCatchMissingFieldsExceptionWhenFieldIsMissing(string name, string surname, string email)
        {
            var guestDTO = new GuestDTO
            {
                Id = 0,
                Name = name,
                Surname = surname,
                Email = email,
                IdNumber = "123456",
                DocumentType = 1,
            };

            var request = new CreateGuestRequest { Data = guestDTO };

            var response = await guestManager.CreateGuest(request);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(ErrorCode.MISSING_REQUIRED_INFORMATION, response.ErrorCode);
            Assert.AreEqual("Missing required information", response.Message);
        }

        [Test]
        public async Task ShouldCatchInvalidFieldExceptionWhenEmailIsInvalid()
        {
            var guestDTO = new GuestDTO
            {
                Id = 0,
                Name = "Some",
                Surname = "Guest",
                Email = "guest.com",
                IdNumber = "123456",
                DocumentType = 1,
            };

            var request = new CreateGuestRequest { Data = guestDTO };

            var response = await guestManager.CreateGuest(request);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(ErrorCode.INVALID_FIELD, response.ErrorCode);
            Assert.AreEqual("Invalid information: email", response.Message);
        }

        [Test]
        public async Task ShouldCatchGenericExceptionWhenDataCannotBeStored()
        {

            var fakeRepository = new Mock<IGuestRepository>();
            fakeRepository.Setup(x => x.Create(It.IsAny<Guest>()))
                .Throws(new Exception());
            guestManager = new GuestManager(fakeRepository.Object);

            var guestDTO = new GuestDTO
            {
                Id = 0,
                Name = "Some",
                Surname = "Guest",
                Email = "guest@email.com",
                IdNumber = "123456",
                DocumentType = 1,
            };

            var request = new CreateGuestRequest { Data = guestDTO };

            var response = await guestManager.CreateGuest(request);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Sucess);
            Assert.AreEqual(ErrorCode.COULD_NOT_STORE_DATA, response.ErrorCode);
            Assert.AreEqual("Something went wrong while trying to save guest to database", response.Message);
        }
    }
}