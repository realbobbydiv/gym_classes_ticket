using GymClassBooking.DAL.Entities;
using GymClassBooking.DAL.Mongo;
using MongoDB.Driver;

namespace GymClassBooking.DAL.Repositories;

public class ClassSessionRepository : Interfaces.IClassSessionRepository
{
    private readonly MongoContext _ctx;

    public ClassSessionRepository(MongoContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<List<ClassSessionEntity>> GetAllAsync(CancellationToken ct)
        => await _ctx.ClassSessions.Find(_ => true).ToListAsync(ct);

    public async Task<ClassSessionEntity?> GetByIdAsync(string id, CancellationToken ct)
        => await _ctx.ClassSessions.Find(x => x.Id == id).FirstOrDefaultAsync(ct);

    public async Task<ClassSessionEntity> CreateAsync(ClassSessionEntity entity, CancellationToken ct)
    {
        await _ctx.ClassSessions.InsertOneAsync(entity, cancellationToken: ct);
        return entity;
    }

    public async Task<bool> UpdateAsync(ClassSessionEntity entity, CancellationToken ct)
    {
        var result = await _ctx.ClassSessions.ReplaceOneAsync(x => x.Id == entity.Id, entity, cancellationToken: ct);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken ct)
    {
        var result = await _ctx.ClassSessions.DeleteOneAsync(x => x.Id == id, ct);
        return result.DeletedCount > 0;
    }
}
