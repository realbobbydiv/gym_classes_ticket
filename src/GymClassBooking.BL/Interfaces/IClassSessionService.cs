using GymClassBooking.BL.Dtos;

namespace GymClassBooking.BL.Interfaces;

public interface IClassSessionService
{
    Task<List<ClassSessionDto>> GetAllAsync(CancellationToken ct);
    Task<ClassSessionDto> GetByIdAsync(string id, CancellationToken ct);
    Task<ClassSessionDto> CreateAsync(ClassSessionDto dto, CancellationToken ct);
    Task<ClassSessionDto> UpdateAsync(string id, ClassSessionDto dto, CancellationToken ct);
    Task DeleteAsync(string id, CancellationToken ct);
}
