namespace GymClassBooking.Host.Contracts.Requests.Classes;

public class ClassSessionResponse
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime StartDateUtc { get; set; }
    public decimal Price { get; set; }
    public int AvailableSpots { get; set; }
    public bool IsActive { get; set; } = true;
}
