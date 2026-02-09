namespace GymClassBooking.BL.Options;

public class BookingOptions
{
    public int MaxSpotsPerUser { get; set; } = 5;
    public decimal BookingFeePercent { get; set; } = 0m; // 0.05 = 5%
    public bool AllowBookingAfterStart { get; set; } = false;
}
