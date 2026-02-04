using EventTicketing.BL.Dtos;
using EventTicketing.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventTicketing.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventsService _eventsService;

    public EventsController(IEventsService eventsService)
    {
        _eventsService = eventsService;
    }

    [HttpGet]
    public async Task<ActionResult<List<EventDto>>> GetAll(CancellationToken ct)
    {
        var result = await _eventsService.GetAllAsync(ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetById(string id, CancellationToken ct)
    {
        var result = await _eventsService.GetByIdAsync(id, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<EventDto>> Create([FromBody] EventDto dto, CancellationToken ct)
    {
        var created = await _eventsService.CreateAsync(dto, ct);
        return Ok(created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EventDto>> Update(string id, [FromBody] EventDto dto, CancellationToken ct)
    {
        var updated = await _eventsService.UpdateAsync(id, dto, ct);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        await _eventsService.DeleteAsync(id, ct);
        return NoContent();
    }
}
