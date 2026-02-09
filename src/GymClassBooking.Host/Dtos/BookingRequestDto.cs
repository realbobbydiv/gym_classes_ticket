namespace GymClassBooking.Host.Dtos;

public class BookingRequestDto
{
    public string UserId { get; set; } = string.Empty;
    public string ClassSessionId { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
