using EventTicketing.BL.Dtos;
using EventTicketing.BL.Exceptions;
using EventTicketing.BL.Interfaces;
using EventTicketing.DAL.Entities;
using EventTicketing.DAL.Interfaces;

namespace EventTicketing.BL.Services;

public class EventsService : IEventsService
{
    private readonly IEventRepository _events;

    public EventsService(IEventRepository events)
    {
        _events = events;
    }

    public async Task<List<EventDto>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await _events.GetAllAsync(ct);
        return items.Select(ToDto).ToList();
    }

    public async Task<EventDto> GetByIdAsync(string id, CancellationToken ct = default)
    {
        var entity = await _events.GetByIdAsync(id, ct);
        if (entity is null)
            throw new NotFoundException($"Event with id '{id}' was not found.");

        return ToDto(entity);
    }

    public async Task<EventDto> CreateAsync(EventDto dto, CancellationToken ct = default)
    {
        var entity = new EventEntity
        {
            Name = dto.Name,
            Location = dto.Location,
            StartDateUtc = dto.StartDateUtc,
            Price = dto.Price,
            AvailableTickets = dto.AvailableTickets,
            IsActive = dto.IsActive
        };

        await _events.CreateAsync(entity, ct);
        return ToDto(entity);
    }

    public async Task<EventDto> UpdateAsync(string id, EventDto dto, CancellationToken ct = default)
    {
        var existing = await _events.GetByIdAsync(id, ct);
        if (existing is null)
            throw new NotFoundException($"Event with id '{id}' was not found.");

        existing.Name = dto.Name;
        existing.Location = dto.Location;
        existing.StartDateUtc = dto.StartDateUtc;
        existing.Price = dto.Price;
        existing.AvailableTickets = dto.AvailableTickets;
        existing.IsActive = dto.IsActive;

        await _events.UpdateAsync(existing, ct);
        return ToDto(existing);
    }

    public async Task DeleteAsync(string id, CancellationToken ct = default)
    {
        var existing = await _events.GetByIdAsync(id, ct);
        if (existing is null)
            throw new NotFoundException($"Event with id '{id}' was not found.");

        await _events.DeleteAsync(id, ct);
    }

    private static EventDto ToDto(EventEntity e) => new()
    {
        Id = e.Id,
        Name = e.Name,
        Location = e.Location,
        StartDateUtc = e.StartDateUtc,
        Price = e.Price,
        AvailableTickets = e.AvailableTickets,
        IsActive = e.IsActive
    };
}
