using EventTicketing.DAL.Entities;
using EventTicketing.DAL.Mongo;
using MongoDB.Driver;

namespace EventTicketing.DAL.Repositories;

public class EventRepository : Interfaces.IEventRepository
{
    private readonly MongoContext _ctx;

    public EventRepository(MongoContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<List<EventEntity>> GetAllAsync(CancellationToken ct)
        => await _ctx.Events.Find(_ => true).ToListAsync(ct);

    public async Task<EventEntity?> GetByIdAsync(string id, CancellationToken ct)
        => await _ctx.Events.Find(x => x.Id == id).FirstOrDefaultAsync(ct);

    public async Task<EventEntity> CreateAsync(EventEntity entity, CancellationToken ct)
    {
        await _ctx.Events.InsertOneAsync(entity, cancellationToken: ct);
        return entity;
    }

    public async Task<bool> UpdateAsync(EventEntity entity, CancellationToken ct)
    {
        var result = await _ctx.Events.ReplaceOneAsync(x => x.Id == entity.Id, entity, cancellationToken: ct);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken ct)
    {
        var result = await _ctx.Events.DeleteOneAsync(x => x.Id == id, ct);
        return result.DeletedCount > 0;
    }
}
