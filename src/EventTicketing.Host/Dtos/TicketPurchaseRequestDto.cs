namespace EventTicketing.Host.Dtos;

public class TicketPurchaseRequestDto
{
    public string UserId { get; set; } = string.Empty;
    public string EventId { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
