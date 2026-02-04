using EventTicketing.DAL.Entities;

namespace EventTicketing.DAL.Interfaces;

public interface IEventRepository
{
    Task<List<EventEntity>> GetAllAsync(CancellationToken ct);
    Task<EventEntity?> GetByIdAsync(string id, CancellationToken ct);
    Task<EventEntity> CreateAsync(EventEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(EventEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(string id, CancellationToken ct);
}
