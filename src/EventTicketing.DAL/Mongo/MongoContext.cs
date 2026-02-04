using EventTicketing.DAL.Entities;
using MongoDB.Driver;

namespace EventTicketing.DAL.Mongo;

public class MongoContext
{
    private readonly IMongoDatabase _db;

    public MongoContext(IMongoDatabase db)
    {
        _db = db;
    }

    public IMongoCollection<EventEntity> Events => _db.GetCollection<EventEntity>("events");
    public IMongoCollection<UserEntity> Users => _db.GetCollection<UserEntity>("users");
}
