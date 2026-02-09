using GymClassBooking.DAL.Entities;
using MongoDB.Driver;

namespace GymClassBooking.DAL.Mongo;

public class MongoContext
{
    private readonly IMongoDatabase _db;

    public MongoContext(IMongoDatabase db)
    {
        _db = db;
    }

    public IMongoCollection<ClassSessionEntity> ClassSessions => _db.GetCollection<ClassSessionEntity>("classes");
    public IMongoCollection<UserEntity> Users => _db.GetCollection<UserEntity>("users");
}
