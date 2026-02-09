using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GymClassBooking.DAL.Entities;

public class ClassSessionEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public DateTime StartDateUtc { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Price { get; set; }

    public int AvailableSpots { get; set; }
    public bool IsActive { get; set; } = true;
}
