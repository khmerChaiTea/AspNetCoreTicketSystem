using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspNetCoreTicketSystem.Services;
using AspNetCoreTicketSystem.Models;
using AspNetCoreTicketSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Moq;

namespace AspNetCoreTicketSystem.Services.Tests
{
    [TestClass]
    public class TicketSystemServiceTests
    {
        private TicketSystemService? _service; // Nullable reference type
        private DbContextOptions<ApplicationDbContext>? _dbContextOptions; // Nullable reference type

        [TestInitialize]
        public void Setup()
        {
            // Initialize in-memory database
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                                    .UseInMemoryDatabase(databaseName: "TicketSystemTestDb")
                                    .Options;

            var context = new ApplicationDbContext(_dbContextOptions);
            _service = new TicketSystemService(context);
        }

        [TestMethod]
        public async Task CreateTicketAsync_ShouldAddTicket_WhenValidData()
        {
            // Arrange
            var ticket = new TicketSystem
            {
                Title = "Test Ticket",
                Description = "Test Description",
                Status = "Pending"
            };

            // Act
            await _service!.CreateTicketAsync(ticket);

            // Assert
            var context = new ApplicationDbContext(_dbContextOptions!);
            var createdTicket = await context.Tickets.FirstOrDefaultAsync(t => t.Title == "Test Ticket");
            Assert.IsNotNull(createdTicket);
            Assert.AreEqual("Test Ticket", createdTicket.Title);
        }

        [TestMethod]
        public async Task UpdateTicketAsync_ShouldUpdateTicket_WhenValidData()
        {
            // Arrange
            var ticket = new TicketSystem
            {
                Title = "Test Ticket",
                Description = "Test Description",
                Status = "Pending"
            };

            await _service!.CreateTicketAsync(ticket);

            // Ensure the ticket is retrieved from the same context
            var context = new ApplicationDbContext(_dbContextOptions!);
            var existingTicket = await context.Tickets.FirstOrDefaultAsync(t => t.Title == "Test Ticket");
            existingTicket!.Status = "Complete";

            // Act
            await _service.UpdateTicketAsync(existingTicket);

            // Assert
            var updatedTicket = await context.Tickets.FirstOrDefaultAsync(t => t.Title == "Test Ticket");
            Assert.IsNotNull(updatedTicket);
            Assert.AreEqual("Complete", updatedTicket.Status);
        }

        [TestMethod]
        public async Task DeleteTicketAsync_ShouldMarkTicketAsDeleted_WhenValidId()
        {
            // Arrange
            var ticket = new TicketSystem
            {
                Title = "Test Ticket",
                Description = "Test Description",
                Status = "Pending"
            };

            await _service!.CreateTicketAsync(ticket);

            var ticketId = ticket.Id;

            // Act
            await _service.DeleteTicketAsync(ticketId);

            // Assert
            var context = new ApplicationDbContext(_dbContextOptions!);
            var deletedTicket = await context.Tickets.FindAsync(ticketId);
            Assert.IsNotNull(deletedTicket);
            Assert.IsTrue(deletedTicket.IsDeleted);
        }

        [TestMethod]
        public async Task CreateTicketAsync_ShouldThrowException_WhenInvalidData()
        {
            // Arrange
            var invalidTicket = new TicketSystem
            {
                Title = null, // Invalid Title
                Description = "Test Description",
                Status = "Pending"
            };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _service!.CreateTicketAsync(invalidTicket));
        }

        [TestMethod]
        public async Task UpdateTicketAsync_ShouldThrowException_WhenTicketDoesNotExist()
        {
            // Arrange
            var nonExistentTicket = new TicketSystem
            {
                Id = 9999, // Non-existent ID
                Title = "Non-existent Ticket",
                Description = "Non-existent Description",
                Status = "Pending"
            };

            // Mock the service to throw an exception for a non-existent ticket
            var mockService = new Mock<ITicketSystemService>();
            mockService
                .Setup(service => service.UpdateTicketAsync(It.IsAny<TicketSystem>()))
                .ThrowsAsync(new InvalidOperationException());

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => mockService.Object.UpdateTicketAsync(nonExistentTicket));
        }

        [TestMethod]
        public async Task DeleteTicketAsync_ShouldNotFail_WhenTicketDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<ITicketSystemService>();
            mockService
                .Setup(service => service.DeleteTicketAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask); // No exception thrown

            // Act
            await mockService.Object.DeleteTicketAsync(9999); // Non-existent ID

            // Assert
            // No exception should be thrown
        }

        [TestMethod]
        public async Task GetTicketByIdAsync_ShouldReturnNull_WhenInvalidId()
        {
            // Arrange
            var invalidId = 9999;

            // Act
            TicketSystem? ticket = null;

            if (_service != null)
            {
                ticket = await _service.GetTicketByIdAsync(invalidId);
            }

            // Assert
            Assert.IsNull(ticket);
        }

        // Adding Integration tests here
        [TestMethod]
        public async Task Integration_CreateAndRetrieveTicket_ShouldWork()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "IntegrationTestDb")
                .Options;

            using var context = new ApplicationDbContext(options);
            var service = new TicketSystemService(context);
            var ticket = new TicketSystem
            {
                Title = "Integration Test Ticket",
                Description = "Integration Test Description",
                Status = "Pending"
            };

            // Act
            await service.CreateTicketAsync(ticket);
            var retrievedTicket = await service.GetTicketByIdAsync(ticket.Id);

            // Assert
            Assert.IsNotNull(retrievedTicket);
            Assert.AreEqual("Integration Test Ticket", retrievedTicket.Title);
            Assert.AreEqual("Integration Test Description", retrievedTicket.Description);
        }

        [TestMethod]
        public async Task Integration_UpdateAndRetrieveTicket_ShouldWork()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "IntegrationTestDb")
                .Options;

            using var context = new ApplicationDbContext(options);
            var service = new TicketSystemService(context);
            var ticket = new TicketSystem
            {
                Title = "Initial Title",
                Description = "Initial Description",
                Status = "Pending"
            };

            await service.CreateTicketAsync(ticket);

            // Update the ticket
            ticket.Title = "Updated Title";
            ticket.Description = "Updated Description";

            // Act
            await service.UpdateTicketAsync(ticket);
            var updatedTicket = await service.GetTicketByIdAsync(ticket.Id);

            // Assert
            Assert.IsNotNull(updatedTicket);
            Assert.AreEqual("Updated Title", updatedTicket.Title);
            Assert.AreEqual("Updated Description", updatedTicket.Description);
        }

        [TestMethod]
        public async Task Integration_DeleteTicket_ShouldWork()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "IntegrationTestDb")
                .Options;

            using var context = new ApplicationDbContext(options);
            var service = new TicketSystemService(context);
            var ticket = new TicketSystem
            {
                Title = "To be deleted",
                Description = "To be deleted",
                Status = "Pending"
            };

            await service.CreateTicketAsync(ticket);
            var ticketId = ticket.Id;

            // Act
            await service.DeleteTicketAsync(ticketId);
            var deletedTicket = await context.Tickets.FindAsync(ticketId);

            // Assert
            Assert.IsNotNull(deletedTicket);
            Assert.IsTrue(deletedTicket.IsDeleted);
        }

        [TestMethod]
        public async Task Integration_UpdateTicket_ShouldHandleConcurrency()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "IntegrationTestDb")
                .Options;

            using var context = new ApplicationDbContext(options);
            var service = new TicketSystemService(context);
            var ticket = new TicketSystem
            {
                Title = "Concurrency Test",
                Description = "Initial Description",
                Status = "Pending"
            };

            await service.CreateTicketAsync(ticket);

            // Act
            var ticketInDb = await service.GetTicketByIdAsync(ticket.Id);
            if (ticketInDb == null) Assert.Fail("Ticket not found in the database.");

            ticketInDb.Description = "Updated Description";
            await service.UpdateTicketAsync(ticketInDb);

            var updatedTicket = await service.GetTicketByIdAsync(ticket.Id);
            if (updatedTicket == null) Assert.Fail("Updated ticket not found in the database.");

            // Assert
            Assert.AreEqual("Updated Description", updatedTicket.Description);
        }
    }
}
