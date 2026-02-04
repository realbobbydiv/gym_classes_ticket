namespace EventTicketing.Host.Contracts.Requests.Tickets;

public class PurchaseTicketRequest
{
    public string UserId { get; set; } = string.Empty;
    public string EventId { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
