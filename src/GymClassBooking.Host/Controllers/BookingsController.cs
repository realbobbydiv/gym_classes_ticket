using GymClassBooking.BL.Dtos;
using GymClassBooking.BL.Interfaces;
using GymClassBooking.Host.Dtos;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace GymClassBooking.Host.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _ticketService;
    private readonly IValidator<BookingRequestDto> _validator;

    public BookingsController(IBookingService ticketService, IValidator<BookingRequestDto> validator)
    {
        _ticketService = ticketService;
        _validator = validator;
    }

    [HttpPost("book")]
    public async Task<ActionResult<BookingResultDto>> Book([FromBody] BookingRequestDto dto, CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(dto, ct);
        if (!validation.IsValid)
            return BadRequest(validation.Errors);

        var result = await _ticketService.BookAsync(dto.UserId, dto.ClassSessionId, dto.Quantity, ct);
        return Ok(result);
    }
}
