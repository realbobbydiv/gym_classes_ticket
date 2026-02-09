using GymClassBooking.DAL.Entities;

namespace GymClassBooking.DAL.Interfaces;

public interface IClassSessionRepository
{
    Task<List<ClassSessionEntity>> GetAllAsync(CancellationToken ct);
    Task<ClassSessionEntity?> GetByIdAsync(string id, CancellationToken ct);
    Task<ClassSessionEntity> CreateAsync(ClassSessionEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ClassSessionEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(string id, CancellationToken ct);
}
