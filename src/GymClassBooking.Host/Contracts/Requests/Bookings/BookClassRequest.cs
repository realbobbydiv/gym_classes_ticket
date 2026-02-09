namespace GymClassBooking.Host.Contracts.Requests.Bookings;

public class BookClassRequest
{
    public string UserId { get; set; } = string.Empty;
    public string ClassSessionId { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
