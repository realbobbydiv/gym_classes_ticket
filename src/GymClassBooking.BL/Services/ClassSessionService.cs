using GymClassBooking.BL.Dtos;
using GymClassBooking.BL.Exceptions;
using GymClassBooking.BL.Interfaces;
using GymClassBooking.DAL.Entities;
using GymClassBooking.DAL.Interfaces;

namespace GymClassBooking.BL.Services;

public class ClassSessionService : IClassSessionService
{
    private readonly IClassSessionRepository _classes;

    public ClassSessionService(IClassSessionRepository classes)
    {
        _classes = classes;
    }

    public async Task<List<ClassSessionDto>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await _classes.GetAllAsync(ct);
        return items.Select(ToDto).ToList();
    }

    public async Task<ClassSessionDto> GetByIdAsync(string id, CancellationToken ct = default)
    {
        var entity = await _classes.GetByIdAsync(id, ct);
        if (entity is null)
            throw new NotFoundException($"Class session with id '{id}' was not found.");

        return ToDto(entity);
    }

    public async Task<ClassSessionDto> CreateAsync(ClassSessionDto dto, CancellationToken ct = default)
    {
        var entity = new ClassSessionEntity
        {
            Name = dto.Name,
            Location = dto.Location,
            StartDateUtc = dto.StartDateUtc,
            Price = dto.Price,
            AvailableSpots = dto.AvailableSpots,
            IsActive = dto.IsActive
        };

        await _classes.CreateAsync(entity, ct);
        return ToDto(entity);
    }

    public async Task<ClassSessionDto> UpdateAsync(string id, ClassSessionDto dto, CancellationToken ct = default)
    {
        var existing = await _classes.GetByIdAsync(id, ct);
        if (existing is null)
            throw new NotFoundException($"Class session with id '{id}' was not found.");

        existing.Name = dto.Name;
        existing.Location = dto.Location;
        existing.StartDateUtc = dto.StartDateUtc;
        existing.Price = dto.Price;
        existing.AvailableSpots = dto.AvailableSpots;
        existing.IsActive = dto.IsActive;

        await _classes.UpdateAsync(existing, ct);
        return ToDto(existing);
    }

    public async Task DeleteAsync(string id, CancellationToken ct = default)
    {
        var existing = await _classes.GetByIdAsync(id, ct);
        if (existing is null)
            throw new NotFoundException($"Class session with id '{id}' was not found.");

        await _classes.DeleteAsync(id, ct);
    }

    private static ClassSessionDto ToDto(ClassSessionEntity e) => new()
    {
        Id = e.Id,
        Name = e.Name,
        Location = e.Location,
        StartDateUtc = e.StartDateUtc,
        Price = e.Price,
        AvailableSpots = e.AvailableSpots,
        IsActive = e.IsActive
    };
}
