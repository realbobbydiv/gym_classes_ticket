namespace GymClassBooking.Host.Contracts.Requests.Bookings;

public class BookingResponse
{
    public string ClassSessionId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public int RemainingSpots { get; set; }
}
