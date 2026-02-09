using GymClassBooking.BL.Dtos;
using GymClassBooking.BL.Exceptions;
using GymClassBooking.BL.Interfaces;
using GymClassBooking.DAL.Interfaces;
using GymClassBooking.BL.Options;
using Microsoft.Extensions.Options;

namespace GymClassBooking.BL.Services;

public class BookingService : IBookingService
{
    private readonly IClassSessionRepository _classes;
    private readonly IUserRepository _users;
    private readonly IOptionsMonitor<BookingOptions> _options;

    public BookingService(
        IClassSessionRepository classes,
        IUserRepository users,
        IOptionsMonitor<BookingOptions> options)
    {
        _classes = classes;
        _users = users;
        _options = options;
    }

    public async Task<BookingResultDto> BookAsync(string userId, string classSessionId, int quantity, CancellationToken ct = default)
    {
        if (quantity <= 0)
            throw new BusinessRuleException("Quantity must be greater than 0.");

        var user = await _users.GetByIdAsync(userId, ct);
        if (user is null)
            throw new NotFoundException($"User with id '{userId}' was not found.");

        var session = await _classes.GetByIdAsync(classSessionId, ct);
        if (session is null)
            throw new NotFoundException($"Class session with id '{classSessionId}' was not found.");

        var opts = _options.CurrentValue;

        if (!opts.AllowBookingAfterStart && session.StartDateUtc <= DateTime.UtcNow)
            throw new BusinessRuleException("Class session already started; booking not allowed.");

        if (!session.IsActive)
            throw new BusinessRuleException("Class session is not active.");

        if (session.AvailableSpots < quantity)
            throw new BusinessRuleException("Not enough spots available.");

        if (opts.MaxSpotsPerUser > 0 && user.TicketsPurchased + quantity > opts.MaxSpotsPerUser)
            throw new BusinessRuleException("Booking exceeds per-user spot limit.");

        session.AvailableSpots -= quantity;
        await _classes.UpdateAsync(session, ct);

        user.TicketsPurchased += quantity;
        await _users.UpdateAsync(user, ct);

        var subtotal = session.Price * quantity;
        var fee = subtotal * opts.BookingFeePercent;
        var total = subtotal + fee;

        return new BookingResultDto
        {
            ClassSessionId = classSessionId,
            UserId = userId,
            Quantity = quantity,
            TotalPrice = total,
            RemainingSpots = session.AvailableSpots
        };
    }
}
