using EventTicketing.BL.Dtos;
using EventTicketing.BL.Exceptions;
using EventTicketing.BL.Interfaces;
using EventTicketing.DAL.Interfaces;

namespace EventTicketing.BL.Services;

public class TicketService : ITicketService
{
    private readonly IEventRepository _events;
    private readonly IUserRepository _users;

    public TicketService(IEventRepository events, IUserRepository users)
    {
        _events = events;
        _users = users;
    }

    public async Task<TicketPurchaseResultDto> PurchaseAsync(string userId, string eventId, int quantity, CancellationToken ct = default)
    {
        if (quantity <= 0)
            throw new BusinessRuleException("Quantity must be greater than 0.");

        var user = await _users.GetByIdAsync(userId, ct);
        if (user is null)
            throw new NotFoundException($"User with id '{userId}' was not found.");

        var ev = await _events.GetByIdAsync(eventId, ct);
        if (ev is null)
            throw new NotFoundException($"Event with id '{eventId}' was not found.");

        if (!ev.IsActive)
            throw new BusinessRuleException("Event is not active.");

        if (ev.AvailableTickets < quantity)
            throw new BusinessRuleException("Not enough tickets available.");

        ev.AvailableTickets -= quantity;
        await _events.UpdateAsync(ev, ct);

        user.TicketsPurchased += quantity;
        await _users.UpdateAsync(user, ct);

        var total = ev.Price * quantity;

        return new TicketPurchaseResultDto
{
    EventId = eventId,
    UserId = userId,
    Quantity = quantity,
    TotalPrice = total,
    RemainingTickets = ev.AvailableTickets
};

        }
    }

