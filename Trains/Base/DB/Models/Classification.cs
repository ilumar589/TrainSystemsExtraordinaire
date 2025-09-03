namespace Trains.Base.DB.Models;

public sealed class Classification
{
    public Guid              Id { get; set; }
    public required string   Name { get; set; }
    public string?           Description { get; set; }
    public required DateTime LastUpdatedAt { get; set; }
    public required string   LastUpdatedBy { get; set; }
}
