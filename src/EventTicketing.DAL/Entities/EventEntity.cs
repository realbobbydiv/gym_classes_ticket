using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventTicketing.DAL.Entities;

public class EventEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public DateTime StartDateUtc { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Price { get; set; }

    public int AvailableTickets { get; set; }
    public bool IsActive { get; set; } = true;
}
