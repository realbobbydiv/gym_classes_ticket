namespace EventTicketing.Host.Options;

public class TicketingOptions
{
    public int MaxTicketsPerUser { get; set; } = 5;
    public decimal ServiceFeePercent { get; set; } = 0m; // 0.05 = 5%
    public bool AllowPurchasesAfterStart { get; set; } = false;
}
