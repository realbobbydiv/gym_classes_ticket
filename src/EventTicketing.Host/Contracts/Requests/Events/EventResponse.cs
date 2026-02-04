namespace EventTicketing.Host.Contracts.Responses.Events;

public class EventResponse
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime StartDateUtc { get; set; }
    public decimal Price { get; set; }
    public int AvailableTickets { get; set; }
    public bool IsActive { get; set; }
}
