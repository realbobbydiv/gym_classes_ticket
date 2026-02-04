namespace EventTicketing.Host.Contracts.Responses.Tickets;

public class TicketPurchaseResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;

    public string EventId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;

    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public int RemainingTickets { get; set; }
}
