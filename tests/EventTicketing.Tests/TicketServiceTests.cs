using EventTicketing.BL.Exceptions;
using EventTicketing.BL.Services;
using EventTicketing.DAL.Entities;
using EventTicketing.DAL.Interfaces;
using EventTicketing.Host.Options;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace EventTicketing.Tests;

public class TicketServiceTests
{
    [Fact]
    public async Task PurchaseAsync_WhenValid_ShouldReturnResult()
    {
        // Arrange
        var eventRepo = new Mock<IEventRepository>();
        var userRepo = new Mock<IUserRepository>();

        var ev = new EventEntity
        {
            Id = "event1",
            Name = "Concert",
            Location = "Sofia",
            StartDateUtc = DateTime.UtcNow.AddDays(5),
            Price = 10m,
            AvailableTickets = 10,
            IsActive = true
        };

        var user = new UserEntity
        {
            Id = "user1",
            FullName = "Test User",
            Email = "test@test.com",
            TicketsPurchased = 0
        };

        eventRepo.Setup(x => x.GetByIdAsync("event1", It.IsAny<CancellationToken>())).ReturnsAsync(ev);
        userRepo.Setup(x => x.GetByIdAsync("user1", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        eventRepo.Setup(x => x.UpdateAsync(It.IsAny<EventEntity>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        userRepo.Setup(x => x.UpdateAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var options = new Mock<IOptionsMonitor<TicketingOptions>>();
        options.Setup(o => o.CurrentValue).Returns(new TicketingOptions
        {
            MaxTicketsPerUser = 5,
            ServiceFeePercent = 0m,
            AllowPurchasesAfterStart = false
        });

        var service = new TicketService(eventRepo.Object, userRepo.Object, options.Object);

        // Act
        var result = await service.PurchaseAsync("user1", "event1", 2, CancellationToken.None);

        // Assert
        Assert.Equal("event1", result.EventId);
        Assert.Equal("user1", result.UserId);
        Assert.Equal(2, result.Quantity);
        Assert.Equal(20m, result.TotalPrice);
        Assert.Equal(8, result.RemainingTickets);

        eventRepo.Verify(x => x.UpdateAsync(It.IsAny<EventEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        userRepo.Verify(x => x.UpdateAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task PurchaseAsync_WhenNotEnoughTickets_ShouldThrow()
    {
        // Arrange
        var eventRepo = new Mock<IEventRepository>();
        var userRepo = new Mock<IUserRepository>();

        var ev = new EventEntity
        {
            Id = "event1",
            Name = "Concert",
            Location = "Sofia",
            StartDateUtc = DateTime.UtcNow.AddDays(5),
            Price = 10m,
            AvailableTickets = 1,
            IsActive = true
        };

        var user = new UserEntity
        {
            Id = "user1",
            FullName = "Test User",
            Email = "test@test.com",
            TicketsPurchased = 0
        };

        eventRepo.Setup(x => x.GetByIdAsync("event1", It.IsAny<CancellationToken>())).ReturnsAsync(ev);
        userRepo.Setup(x => x.GetByIdAsync("user1", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var options = new Mock<IOptionsMonitor<TicketingOptions>>();
        options.Setup(o => o.CurrentValue).Returns(new TicketingOptions
        {
            MaxTicketsPerUser = 5,
            ServiceFeePercent = 0m,
            AllowPurchasesAfterStart = false
        });

        var service = new TicketService(eventRepo.Object, userRepo.Object, options.Object);

        // Act + Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            service.PurchaseAsync("user1", "event1", 2, CancellationToken.None));
    }
}
