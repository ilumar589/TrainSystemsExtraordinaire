namespace Trains.Base.DB.Models;

public sealed class DeviceType
{
    public Guid              Id { get; set; }
    public required string   Name { get; set; }
    public required string   Manufacturer { get; set; }
    public required string   Model { get; set; }
    public string?           Description { get; set; }
    public required DateTime LastUpdatedAt { get; set; }
    public required string   LastUpdatedBy { get; set; }
}
