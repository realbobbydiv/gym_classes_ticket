using EventTicketing.BL.Dtos;
using EventTicketing.BL.Interfaces;
using EventTicketing.Host.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EventTicketing.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpPost("purchase")]
    public async Task<ActionResult<TicketPurchaseResultDto>> Purchase([FromBody] TicketPurchaseRequestDto dto, CancellationToken ct)
    {
        var result = await _ticketService.PurchaseAsync(dto.UserId, dto.EventId, dto.Quantity, ct);
        return Ok(result);
    }
}
