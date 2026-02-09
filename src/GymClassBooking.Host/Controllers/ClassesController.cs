using GymClassBooking.BL.Dtos;
using GymClassBooking.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymClassBooking.Host.Controllers;

[ApiController]
[Route("api/classes")]
public class ClassesController : ControllerBase
{
    private readonly IClassSessionService _classesService;

    public ClassesController(IClassSessionService classesService)
    {
        _classesService = classesService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ClassSessionDto>>> GetAll(CancellationToken ct)
    {
        var result = await _classesService.GetAllAsync(ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClassSessionDto>> GetById(string id, CancellationToken ct)
    {
        var result = await _classesService.GetByIdAsync(id, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ClassSessionDto>> Create([FromBody] ClassSessionDto dto, CancellationToken ct)
    {
        var created = await _classesService.CreateAsync(dto, ct);
        return Ok(created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ClassSessionDto>> Update(string id, [FromBody] ClassSessionDto dto, CancellationToken ct)
    {
        var updated = await _classesService.UpdateAsync(id, dto, ct);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        await _classesService.DeleteAsync(id, ct);
        return NoContent();
    }
}
