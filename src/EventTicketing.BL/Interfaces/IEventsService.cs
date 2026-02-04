using EventTicketing.BL.Dtos;

namespace EventTicketing.BL.Interfaces;

public interface IEventsService
{
    Task<List<EventDto>> GetAllAsync(CancellationToken ct);
    Task<EventDto> GetByIdAsync(string id, CancellationToken ct);
    Task<EventDto> CreateAsync(EventDto dto, CancellationToken ct);
    Task<EventDto> UpdateAsync(string id, EventDto dto, CancellationToken ct);
    Task DeleteAsync(string id, CancellationToken ct);
}
