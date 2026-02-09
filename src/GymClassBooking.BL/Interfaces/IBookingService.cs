using GymClassBooking.BL.Dtos;

namespace GymClassBooking.BL.Interfaces;

public interface IBookingService
{
    Task<BookingResultDto> BookAsync(string userId, string classSessionId, int quantity, CancellationToken ct);
}
