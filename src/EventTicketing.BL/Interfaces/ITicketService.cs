using EventTicketing.BL.Dtos;

namespace EventTicketing.BL.Interfaces;

public interface ITicketService
{
    Task<TicketPurchaseResultDto> PurchaseAsync(string userId, string eventId, int quantity, CancellationToken ct);
}
