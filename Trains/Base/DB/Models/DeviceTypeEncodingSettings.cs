namespace Trains.Base.DB.Models;

public sealed class DeviceTypeEncodingSettings
{
    public Guid                    Id { get; set; }
    public required CameraSettings CameraSettings { get; set; }
    public required DateTime       LastUpdatedAt { get; set; }
    public required string         LastUpdatedBy { get; set; }
}
