namespace EventTicketing.Host.Contracts.Requests.Events;

public class AddEventRequest
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime StartDateUtc { get; set; }
    public decimal Price { get; set; }
    public int AvailableTickets { get; set; }
    public bool IsActive { get; set; } = true;
}
