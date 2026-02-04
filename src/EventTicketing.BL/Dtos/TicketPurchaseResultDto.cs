namespace EventTicketing.BL.Dtos;

public class TicketPurchaseResultDto
{
    public string EventId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public int RemainingTickets { get; set; }
}
