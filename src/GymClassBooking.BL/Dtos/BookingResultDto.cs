namespace GymClassBooking.BL.Dtos;

public class BookingResultDto
{
    public string ClassSessionId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public int RemainingSpots { get; set; }
}
