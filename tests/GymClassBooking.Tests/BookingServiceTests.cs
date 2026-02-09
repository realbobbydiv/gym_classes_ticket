using GymClassBooking.BL.Exceptions;
using GymClassBooking.BL.Services;
using GymClassBooking.DAL.Entities;
using GymClassBooking.DAL.Interfaces;
using GymClassBooking.BL.Options;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace GymClassBooking.Tests;

public class BookingServiceTests
{
    [Fact]
    public async Task BookAsync_WhenValid_ShouldReturnResult()
    {
        // Arrange
        var classRepo = new Mock<IClassSessionRepository>();
        var userRepo = new Mock<IUserRepository>();

        var ev = new ClassSessionEntity
        {
            Id = "event1",
            Name = "Concert",
            Location = "Sofia",
            StartDateUtc = DateTime.UtcNow.AddDays(5),
            Price = 10m,
            AvailableSpots = 10,
            IsActive = true
        };

        var user = new UserEntity
        {
            Id = "user1",
            FullName = "Test User",
            Email = "test@test.com",
            TicketsPurchased = 0
        };

        classRepo.Setup(x => x.GetByIdAsync("event1", It.IsAny<CancellationToken>())).ReturnsAsync(ev);
        userRepo.Setup(x => x.GetByIdAsync("user1", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        classRepo.Setup(x => x.UpdateAsync(It.IsAny<ClassSessionEntity>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        userRepo.Setup(x => x.UpdateAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var options = new Mock<IOptionsMonitor<BookingOptions>>();
        options.Setup(o => o.CurrentValue).Returns(new BookingOptions
        {
            MaxSpotsPerUser = 5,
            BookingFeePercent = 0m,
            AllowBookingAfterStart = false
        });

        var service = new BookingService(classRepo.Object, userRepo.Object, options.Object);

        // Act
        var result = await service.BookAsync("user1", "event1", 2, CancellationToken.None);

        // Assert
        Assert.Equal("event1", result.ClassSessionId);
        Assert.Equal("user1", result.UserId);
        Assert.Equal(2, result.Quantity);
        Assert.Equal(20m, result.TotalPrice);
        Assert.Equal(8, result.RemainingSpots);

        classRepo.Verify(x => x.UpdateAsync(It.IsAny<ClassSessionEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        userRepo.Verify(x => x.UpdateAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task BookAsync_WhenNotEnoughSpots_ShouldThrow()
    {
        // Arrange
        var classRepo = new Mock<IClassSessionRepository>();
        var userRepo = new Mock<IUserRepository>();

        var ev = new ClassSessionEntity
        {
            Id = "event1",
            Name = "Concert",
            Location = "Sofia",
            StartDateUtc = DateTime.UtcNow.AddDays(5),
            Price = 10m,
            AvailableSpots = 1,
            IsActive = true
        };

        var user = new UserEntity
        {
            Id = "user1",
            FullName = "Test User",
            Email = "test@test.com",
            TicketsPurchased = 0
        };

        classRepo.Setup(x => x.GetByIdAsync("event1", It.IsAny<CancellationToken>())).ReturnsAsync(ev);
        userRepo.Setup(x => x.GetByIdAsync("user1", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var options = new Mock<IOptionsMonitor<BookingOptions>>();
        options.Setup(o => o.CurrentValue).Returns(new BookingOptions
        {
            MaxSpotsPerUser = 5,
            BookingFeePercent = 0m,
            AllowBookingAfterStart = false
        });

        var service = new BookingService(classRepo.Object, userRepo.Object, options.Object);

        // Act + Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            service.BookAsync("user1", "event1", 2, CancellationToken.None));
    }
}
